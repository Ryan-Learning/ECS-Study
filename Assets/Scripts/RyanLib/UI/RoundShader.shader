Shader "Unlit/RoundShader"
{
	Properties
	{
		[PerRendererData] _MainTex("Base(RGB)", 2D) = "white" {}
		[HideInInspector]_StencilComp("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask("Stencil Read Mask", Float) = 255
		_ColorMask("Color Mask", Float) = 15
		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
			//("Aspect Ratio", ,Range(-1,1)) = 0
			_AspectRatio("Aspect Ratio",Range(0,2)) = 1
			_RADIUSBUCE("RADIUSBUCE",Range(0,0.5)) = 0.2
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent" // UI存在透明图片，所以渲染队列是 Transparent 
				"IgnoreProjector" = "True" // 忽略投影器Projector
				"RenderType" = "Transparent" // 渲染模式
				"PreviewType" = "Plane" // UI预览的正常都是一个平面(Plane) 
				"CanUseSpriteAtlas" = "True" // UI经常使用图集,所以要设置图集可以使用
			}

			Stencil
			{
				// UI 模板测试，把在Properties中定义的模板参数导入，Unity会自动修改
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off // 剔除关闭
			Lighting Off // 光照关闭
			ZWrite Off // 深度写入关闭
			ZTest[unity_GUIZTestMode] // 用于UI组件的shader都要包含一句：ZTest [unity_GUIZTestMode]，以确保UI能在前层显示
			Blend SrcAlpha OneMinusSrcAlpha // 混合模式是 OneMinusSrcAlpha 正常模式(透明度混合)
			ColorMask[_ColorMask] // 将定义的_ColorMask参数导入

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma exclude_renderers gles
				#pragma multi_compile _ UNITY_UI_ALPHACLIP
				#include "UnityUI.cginc"
				#include "UnityCG.cginc"

				fixed4 _TextureSampleAdd;
				float _AspectRatio;
				float _RADIUSBUCE;
				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 pos : SV_POSITION;
					float4 color : COLOR;
					float2 RadiusBuceVU: TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};
				sampler2D _MainTex;
				v2f vert(appdata_full v)
				{
					v2f o;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);// 实例化处理
					o.pos = UnityObjectToClipPos(v.vertex);// 模型空间到裁剪空间
					o.uv = v.texcoord;
					o.color = v.color;
					o.RadiusBuceVU = v.texcoord - float2(0.5, 0.5);
					return o;
				}

				fixed4 frag(v2f i) : COLOR
				{
				   fixed4 col = (0,1,1,0);

				   if (abs(i.RadiusBuceVU.x) < 0.5 - _RADIUSBUCE || abs(i.RadiusBuceVU.y) < 0.5 - _RADIUSBUCE)
				   {
					   col = tex2D(_MainTex, i.uv) * i.color;
				   }
				   else
				   {
					   if (length(abs(i.RadiusBuceVU) - float2(0.5 - _RADIUSBUCE, 0.5 - _RADIUSBUCE)) < _RADIUSBUCE)
					   {
						   col = tex2D(_MainTex, i.uv) * i.color;
					   }
					   else
					   {
						   discard;
					   }
				   }
				   return col;
				}
				ENDCG
			}
		}
}
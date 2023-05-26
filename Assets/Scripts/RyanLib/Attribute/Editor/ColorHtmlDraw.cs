/*************************************************
  * 名稱：ColorHtmlDraw
  * 作者：RyanHsu
  * 功能說明：格式化Color為ToHtmlStringRGBA
  * ***********************************************/
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ColorHtmlAttribute))]
public class ColorHtmlDraw : PropertyDrawer
{
    ColorHtmlAttribute m_Attribute { get { return ((ColorHtmlAttribute)attribute); } }
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		Rect htmlField = new Rect(position.x, position.y, position.width - 100, position.height);
		Rect colorField = new Rect(position.x + htmlField.width, position.y, position.width - htmlField.width, position.height);

		string htmlValue = EditorGUI.TextField(htmlField, label, "#" + ColorUtility.ToHtmlStringRGBA(property.colorValue));

		Color newCol;
		if (ColorUtility.TryParseHtmlString(htmlValue, out newCol))
			property.colorValue = newCol;

		property.colorValue = EditorGUI.ColorField(colorField, property.colorValue);
	}

}
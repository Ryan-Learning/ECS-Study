/*************************************************
  * 名稱：UIFollowMouse
  * 作者：RyanHsu
  * 功能說明：Txt跟隨鼠標並顯示座標值
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using TMPro;
using UnityEngine.UI;

namespace RyanLib.UI
{

    public class UIFollow_MousePositionTxt : MonoBehaviour
    {
        //public TextMeshProUGUI textMeshPro;
        public Text textMeshPro;

        void Start() { }

        void Update()
        {
            transform.position = Input.mousePosition;
            if (textMeshPro) textMeshPro.text = "(X:" + transform.position.x.ToString("0.00") + "Y:" + transform.position.y.ToString("0.00") + ")";
        }

    }

}

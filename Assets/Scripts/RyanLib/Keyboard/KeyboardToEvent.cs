/*************************************************
 * 作者：RyanHsu
 * 腳本說明：GameObject鍵盤事件的封裝程式，針對全局keyboard Event做設定
 * ***********************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;
using System;
using ExtensionMethods;

//public interface IKeyboard { }
namespace RyanLib.Keyboard
{
    [Serializable]
    public class KeyboardGroup
    {
        [Header("快捷名稱")] public string name = "";
        [Tooltip("激活")] public bool io = true;
        public KeyCode code1 = KeyCode.None;
        public KeyCode code2 = KeyCode.None;
        public UnityEvent OnKeyDown;
    }

    public class KeyboardToEvent : MonoBehaviour
    {
        public List<KeyboardGroup> keyboardGroup;

        void Start() { }

        void Update()
        {
            foreach (KeyboardGroup k in keyboardGroup) {
                if (!k.io) break;

                bool keyIO = false;
                if (k.code1 == KeyCode.None) keyIO = Input.GetKeyDown(k.code2);
                else if (k.code2 == KeyCode.None) keyIO = Input.GetKeyDown(k.code1);
                else keyIO = (Input.GetKey(k.code1) && Input.GetKeyDown(k.code2)) || Input.GetKeyDown(k.code1) && Input.GetKey(k.code2);

                if (keyIO) {
                    k.OnKeyDown.Invoke();
                    UnityEngine.Object obj = k.OnKeyDown.GetPersistentTarget(0);
                }
            }
        }


#if UNITY_EDITOR
        void OnValidate()
        {

        }
#endif
    }
}
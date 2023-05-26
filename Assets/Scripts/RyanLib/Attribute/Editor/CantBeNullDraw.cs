/*************************************************
  * 名稱：CantBeNullDraw
  * 作者：RyanHsu
  * 功能說明：由PropertyDrawer增加Drawer支援
  * ***********************************************/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(CantBeNullAttribute))]
public class CantBeNullDraw : PropertyDrawer
{
    public override void OnGUI(Rect inRect, SerializedProperty inProp, GUIContent label)
    {
        EditorGUI.BeginProperty(inRect, label, inProp);

        bool error = inProp.objectReferenceValue == null;
        if (error)
        {
            label.text = "[!] " + label.text;
            GUI.color = Color.red;
        }

        EditorGUI.PropertyField(inRect, inProp, label);
        GUI.color = Color.white;

        EditorGUI.EndProperty();
    }
}
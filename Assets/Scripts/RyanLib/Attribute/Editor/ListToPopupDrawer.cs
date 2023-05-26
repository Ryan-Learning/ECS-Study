/*************************************************
  * 名稱：ListToPopupDrawer
  * 作者：RyanHsu
  * 功能說明：由PropertyDrawer增加ListToPopupAttribute的Drawer支援
  * ***********************************************/

using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

[CustomPropertyDrawer(typeof(ListToPopupAttribute))]
public class ListToPopupDrawer : PropertyDrawer
{
    SerializedProperty m_List;
    List<string> strList = null;
    Rect _rect;
    Rect rect { get => new Rect(_rect.x + _rect.width * 0.5f, _rect.y, _rect.width - _rect.width * 0.5f, _rect.height); }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ListToPopupAttribute target = attribute as ListToPopupAttribute;
        _rect = position;
        //Attribute取值
        m_List = target.m_ListName == "" ? null : property.serializedObject.FindProperty(target.m_ListName);
        int listCount = 0;
        //List Property解析
        if (m_List.isArray) {
            listCount = m_List.arraySize;
            m_List.Next(true); // skip generic field
            m_List.Next(true); // advance to array size field
            int arrayLength = m_List.intValue;
            target.m_SelectedIndex = Mathf.Clamp(target.m_SelectedIndex, 0, arrayLength);
            m_List.Next(true); // advance to first array index

            if (m_List.propertyType == SerializedPropertyType.String) {
                strList = new List<string>(arrayLength);
                for (int i = 0; i < listCount; i++, m_List.Next(false)) strList.Add(m_List.stringValue);
            }
            else {
                EditorGUI.LabelField(rect, label.text, "請使用List<string>建構popup"); return;
            }
        }
        else {
            EditorGUI.LabelField(rect, label.text, "請使用List<string>建構popup"); return;
        }
        //回傳值
        if (m_List != null && listCount != 0) {

            switch (property.propertyType) {
                case SerializedPropertyType.Float:
                    property.floatValue = target.m_SelectedIndex;
                    EditorGUI.LabelField(position, "(Float)" + label.text + " = "+ property.floatValue);
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = target.m_SelectedIndex;
                    EditorGUI.LabelField(position, "(Int)" + label.text + " = "+ property.intValue);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = strList[target.m_SelectedIndex];
                    EditorGUI.LabelField(position, "(String)" + label.text + " = "+ property.stringValue);
                    break;
                default:
                    Debug.Log("ListToPopup:Property必需為float、int或string");
                    EditorGUI.LabelField(rect, label.text, "ListToPopup: Property必需為float、int或string");
                    break;
            }

            target.m_SelectedIndex = EditorGUI.Popup(rect, target.m_SelectedIndex, strList.ToArray());

        }

    }

}
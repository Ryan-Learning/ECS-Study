/*************************************************
  * 名稱：ReadOnlyDrawer
  * 作者：RyanHsu
  * 功能說明：Inspector ReadOnly套件
  * ***********************************************/
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    SerializedProperty str_value;
    List<int> enum_index;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ReadOnlyAttribute target = attribute as ReadOnlyAttribute;
        enum_index = target.enum_index;

        string newString = target.str_value.Trim(new Char[] { '!' });

        str_value = target.str_value == "" ? null : property.serializedObject.FindProperty(newString);

        GUI.enabled = str_value == null ? false : newString == target.str_value ? checkValue() : !checkValue();
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;

        bool checkValue()
        {
            if (str_value.propertyType == SerializedPropertyType.Boolean) {
                return str_value.boolValue;
            }
            else if (str_value.propertyType == SerializedPropertyType.Enum) {
                return enum_index.Contains(str_value.enumValueFlag);
            }
            return false;
        }
    }


}
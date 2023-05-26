/*************************************************
  * 名稱：DynamicRangeDrawer
  * 作者：RyanHsu
  * 功能說明：由PropertyDrawer增加DynamicRange的Drawer支援
  * ***********************************************/

using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(DynamicRangeAttribute))]
public class DynamicRangeDrawer : PropertyDrawer
{
    SerializedProperty str_max;
    SerializedProperty str_min;
    SerializedProperty str_vector2;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        DynamicRangeAttribute range = attribute as DynamicRangeAttribute;

        str_max = range.str_max == "" ? null : property.serializedObject.FindProperty(range.str_max);
        str_min = range.str_min == "" ? null : property.serializedObject.FindProperty(range.str_min);
        str_vector2 = range.str_vector2 == "" ? null : property.serializedObject.FindProperty(range.str_vector2);

        if (property.propertyType == SerializedPropertyType.Float) {
            range.max = str_max != null ? Convert(str_max) : str_vector2 != null ? ConvertVector(str_vector2).y : range.max;
            range.min = str_min != null ? Convert(str_min) : str_vector2 != null ? ConvertVector(str_vector2).x : range.min;

            EditorGUI.Slider(position, property, range.min, range.max, label);
        }
        else if (property.propertyType == SerializedPropertyType.Integer) {
            range.max = str_max != null ? Convert(str_max) : str_vector2 != null ? ConvertVector(str_vector2).y : range.max;
            range.min = str_min != null ? Convert(str_min) : str_vector2 != null ? ConvertVector(str_vector2).x : range.min;

            EditorGUI.IntSlider(position, property, (int)range.min, (int)range.max, label);
        }
        else {
            EditorGUI.LabelField(position, label.text, "Use Range with float or int.");
        }
    }

    float Convert(SerializedProperty property)
    {
        switch (property.propertyType) {
            case SerializedPropertyType.Generic:
                return (float)property.arraySize - 1;
            case SerializedPropertyType.Integer:
                return (float)property.intValue;
            case SerializedPropertyType.Float:
                return property.floatValue;
            default:
                Debug.Log("DynamicRange:Property必需為float、int、List或Array");
                return 0f;
        }
    }

    Vector2 ConvertVector(SerializedProperty property)
    {
        switch (property.propertyType) {
            case SerializedPropertyType.Vector2:
                return property.vector2Value;
            default:
                //SerializedPropertyType.Generic:
                if (property.isArray) return new Vector2(0f, property.arraySize-1);
                Debug.Log("DynamicRange:Property必需為float、int、List或Array");
                return Vector2.zero;
        }
    }
}
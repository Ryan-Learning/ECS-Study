/*************************************************
  * 名稱：ArrayNameDrawer
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ArrayNameAttribute))]
public class ArrayNameDrawer : PropertyDrawer
{
    public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
    {
        try {
            int pos = int.Parse(property.propertyPath.Split('[', ']')[1]);
            EditorGUI.ObjectField(rect, property, new GUIContent(((ArrayNameAttribute)attribute).names[pos]));
        }
        catch {
            EditorGUI.ObjectField(rect, property, label);
        }
    }
}
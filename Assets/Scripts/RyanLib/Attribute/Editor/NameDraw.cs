/*************************************************
  * 名稱：NameDrawer
  * 作者：RyanHsu
  * 功能說明：只取出Asset Object 的名稱String
  * ***********************************************/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(NameAttribute))]
public class NameDrawer : PropertyDrawer
{
    NameAttribute m_Attribute { get { return ((NameAttribute)attribute); } }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            Object obj = AssetDatabase.LoadAssetAtPath<Object>(property.stringValue);
            if (obj != null)
                property.stringValue = obj.name;
            else
                property.stringValue = "";
        }
        else
            EditorGUI.LabelField(position, label.text, "Use [Name] with Object.");
    }
}
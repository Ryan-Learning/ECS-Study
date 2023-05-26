/*************************************************
  * 名稱：JsonDrawer
  * 作者：RyanHsu
  * 功能說明：由PropertyDrawer增加Json的Drawer支援
  * ***********************************************/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(JsonAttribute))]
public class JsonDrawer : PropertyDrawer
{
    JsonAttribute m_Attribute { get { return ((JsonAttribute)attribute); } }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            TextAsset jsonObject = AssetDatabase.LoadAssetAtPath<TextAsset>(property.stringValue);
            TextAsset json = (TextAsset)EditorGUI.ObjectField(position, label, jsonObject, typeof(TextAsset), false);
            if (json != null)
                property.stringValue = AssetDatabase.GetAssetPath(json);
            else
                property.stringValue = "";
        }
        else
            EditorGUI.LabelField(position, label.text, "Use [Json] with strings.");
    }
}
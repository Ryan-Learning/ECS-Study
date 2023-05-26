/*************************************************
  * 名稱：SceneDrawer
  * 作者：RyanHsu
  * 功能說明：由PropertyDrawer增加Scene的Drawer支援
  * ***********************************************/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SceneAttribute))]
public class SceneDrawer : PropertyDrawer
{
    SceneAttribute m_Attribute { get { return ((SceneAttribute)attribute); } }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String) {
            SceneAsset sceneObject = AssetDatabase.LoadAssetAtPath<SceneAsset>(property.stringValue);

            if (sceneObject == null && !string.IsNullOrEmpty(property.stringValue)) {
                sceneObject = GetBuildSettingsSceneObject(property.stringValue);
            }
            if (sceneObject == null && !string.IsNullOrEmpty(property.stringValue)) {
                Debug.LogError($"Could not find scene {property.stringValue} in the Build Setting List.");
            }
            SceneAsset scene = (SceneAsset)EditorGUI.ObjectField(position, label, sceneObject, typeof(SceneAsset), true);
            if (scene != null) {
                switch (m_Attribute.m_valueType) {
                    case AttributeResult.Type.fullPath://抓取場景路徑
                        property.stringValue = AssetDatabase.GetAssetPath(scene);
                        break;
                    case AttributeResult.Type.name://抓取場景名稱
                        property.stringValue = scene.name;
                        break;
                }
            } else {
                property.stringValue = "";
            }
        } else {
            EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
        }
    }

    protected SceneAsset GetBuildSettingsSceneObject(string sceneName)
    {
        foreach (EditorBuildSettingsScene buildScene in EditorBuildSettings.scenes) {
            SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(buildScene.path);
            if (sceneAsset.name == sceneName) {
                return sceneAsset;
            }
        }
        return null;
    }
}
/*************************************************
  * 名稱：SpaceLineDrawer
  * 作者：RyanHsu
  * 功能說明：
  * ***********************************************/

using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(SpaceLineAttribute))]
public class SpaceLineDrawer : DecoratorDrawer
{
    private const float headerHeight = 30f;

    SpaceLineAttribute target { get => (SpaceLineAttribute)attribute; }

    public override float GetHeight()
    {
        return base.GetHeight() + headerHeight;
    }

    public override void OnGUI(Rect position)
    {
        position.y += headerHeight * 0.5f;
        Rect separatorRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight, position.width, 1.5f);
        EditorGUI.DrawRect(separatorRect, Color.gray);
        position.y -= headerHeight * 0.25f;
        position.x += (EditorGUIUtility.currentViewWidth - target.title.Length * 12f) * 0.5f;
        EditorGUI.LabelField(position, $"{target.title}", EditorStyles.boldLabel);
    }

}
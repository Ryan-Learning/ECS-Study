/*************************************************
  * 名稱：SceneAttribute
  * 作者：RyanHsu
  * 功能說明：SceneDrawer的Attribute標籤屬性格式
  * ***********************************************/
using UnityEngine;

public class SceneAttribute : PropertyAttribute
{
    public readonly AttributeResult.Type m_valueType;

    public SceneAttribute(AttributeResult.Type valueType) { this.m_valueType = valueType; }
}

namespace AttributeResult
{
    public enum Type
    {
        name,
        fullPath,
    }
}
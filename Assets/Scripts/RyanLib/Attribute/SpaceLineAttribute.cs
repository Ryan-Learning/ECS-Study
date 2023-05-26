/*************************************************
  * 名稱：SpaceLineAttribute
  * 作者：RyanHsu
  * 功能說明：在Inspector中繪制一條分隔線
  * ***********************************************/
using System;

namespace UnityEngine
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = true)]
    public class SpaceLineAttribute : PropertyAttribute
    {
        public readonly string title;
        public SpaceLineAttribute(string title = "") { this.title = title; }
    }
}
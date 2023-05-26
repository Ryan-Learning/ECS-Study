/*************************************************
  * 名稱：CantBeNull
  * 作者：RyanHsu
  * 功能說明：Cannot Be Null will red-flood the field if the reference is null.
  * ***********************************************/
using System;
using UnityEngine;

/// <summary> Cannot Be Null will red-flood the field if the reference is null. </summary>
[AttributeUsage(AttributeTargets.Field)]
public class CantBeNullAttribute : PropertyAttribute { }
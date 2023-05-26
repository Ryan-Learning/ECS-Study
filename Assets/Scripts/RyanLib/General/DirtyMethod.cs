/*************************************************
  * 名稱：DirtyMethod
  * 作者：RyanHsu
  * 功能說明：髒運算元
  * ***********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

/// <summary>驗證Method指針</summary>
[Serializable]
public class DirtyMethod<T> where T : class
{
    T dirty = default(T);

    public T defaultValue { set => dirty = value; }

    public bool isDirty(T value)
    {
        if (dirty != value) {
            dirty = value;
            return true;
        }
        else {
            return false;
        }
    }
}

/// <summary>驗證Cloneable或Equatable型式的內容dirty</summary>
public class DirtyEqual<T> where T : ICloneable, IEquatable<T>
{
    T dirty = default(T);
    public T defaultValue { set => dirty = (T)value.Clone(); get => dirty; }

    public bool isDirty(T value)
    {
        if (!dirty.Equals(value)) {
            defaultValue = value;
            return true;
        }
        else {
            return false;
        }
    }
}

[Serializable]
public class DirtyBool
{
    bool dirty;

    public bool defaultValue { set { dirty = value; } }

    public bool isDirty(bool value)
    {
        if (dirty != value) {
            dirty = value;
            return true;
        }
        else {
            return false;
        }
    }
}

[Serializable]
public class Dirtyfloat
{
    float dirty;
    float obsolete;//存放舊指針，用以判斷enter/release

    public float obsoleteValue { get { return obsolete; } }
    public float defaultValue { set { dirty = value; } }

    public bool isDirty(float value)
    {
        if (dirty != value) {
            obsolete = dirty;
            dirty = value;
            return true;
        }
        else {
            return false;
        }
    }
}

[Serializable]
public class DirtyInt
{
    int dirty;
    int obsolete;//存放舊指針，用以判斷enter/release

    public int obsoleteValue { get { return obsolete; } }
    public int defaultValue { set { dirty = value; } }

    public bool isDirty(int value)
    {
        if (dirty != value) {
            obsolete = dirty;
            dirty = value;
            return true;
        }
        else {
            return false;
        }
    }
}

[Serializable]
public class DirtyStr
{
    string dirty;
    string obsolete;//存放舊指針，用以判斷enter/release

    public string obsoleteValue { get { return obsolete; } }
    public string defaultValue { set { dirty = value; } }

    public bool isDirty(string value)
    {
        if (dirty != value) {
            obsolete = dirty;
            dirty = value;
            return true;
        }
        else {
            return false;
        }
    }
}

[Serializable]
public class DirtyVector
{
    Vector4 dirty;
    Vector4 obsolete;//存放舊指針，用以判斷enter/release

    public Vector4 obsoleteValue { get { return obsolete; } }
    public Vector4 defaultValue { set { dirty = value; } }

    public bool isDirty(Vector4 value)
    {
        if (dirty != value) {
            obsolete = dirty;
            dirty = value;
            return true;
        }
        else {
            return false;
        }
    }

}

[Serializable]
public class DirtyParams<T> where T : struct
{
    T[] dirty = default(T[]);
    public T[] defaultValue { set { dirty = value; } }

    public bool isDirty(params T[] value)
    {
        if (dirty == null || dirty.Length != value.Length) {
            dirty = value;
            return true;
        }
        else {
            return !value.SequenceEqual(dirty);
        }
    }

}

[Serializable]
public class DirtyProperty<T> where T : struct
{
    object[] dirty;

    public bool isDirty(object value)
    {
        List<object> properties = new List<object>();
        foreach (var property in value.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public)) {
            if (property.FieldType.IsValueType) {
                properties.Add(property.GetValue(value));
            }
        }
        var propertyArr = properties.ToArray();

        if (dirty == null) {
            dirty = propertyArr;
            return true;
        }
        else {
            return !propertyArr.SequenceEqual(dirty);
        }
    }

}
/*************************************************
  * 名稱：BindableProperty
  * 作者：RyanHsu
  * 功能說明：Binding Data 數據綁定類基底
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Binding Data 數據綁定類基底
/// </summary>
/// <typeparam name="T">數據類別</typeparam>
[Serializable]
public class BindableProperty<T>
{
    [ReadOnly] public string name;
    public delegate void ValueChangeHandler(string name, T oldValue, T newValue);
    /// <summary>OnValueChange事件</summary>
    public ValueChangeHandler OnValueChanged;

    [SerializeField] [ReadOnly] T _value;
    public T value {
        get { return _value; }
        set {
            _value = value;
            if (!Equals(_value, value)) {
                if (OnValueChanged != null) OnValueChanged(name, _value, value);
            }
        }
    }

    /// <summary> BindableProperty初始化 </summary>
    /// <param name="propertyName"> 提供給ValueChangeHandler使用的ObjectName </param>
    /// <param name="value">value初始值</param>
    /// <param name="handler">Value Change事件</param>
    public BindableProperty(string propertyName = "", T value = default, ValueChangeHandler handler = null)
    {
        this.value = value;
        name = propertyName;

        if (handler != null) OnValueChanged += handler;
    }

    public override string ToString()
    {
        return value != null ? value.ToString() : "null";
    }
}

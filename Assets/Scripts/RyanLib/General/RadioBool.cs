/*************************************************
  * 名稱：RadioBool
  * 作者：RyanHsu
  * 功能說明：製作一個支援Radio群組的bool類別
  * ***********************************************/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using ExtensionMethods;

public class RadioBool
{
    public static Dictionary<string, List<RadioBool>> _dic_radio = new Dictionary<string, List<RadioBool>>();
    public delegate void ResultAction(bool boo);
    public ResultAction resultAction = null;

    bool m_io;
    string m_groupName;

    public bool io
    {
        get => m_io;
        set {
            if (m_groupName != null) {//原已有群組的io變更
                if (value) {
                    if (_dic_radio.TryGetValue(m_groupName, out List<RadioBool> radio)) {
                        radio.ForEach(b => b.SetBool(b == this));//輪詢SetBool為是否=this
                    }
                } else {
                    SetBool(false);
                }
            }
            //Debug.Log(this+":"+m_io);
        }
    }

    public string groupName
    {
        get => m_groupName;
        set {
            if (m_groupName != null) {//原已有群組的群組變更
                if (_dic_radio.TryGetValue(m_groupName, out List<RadioBool> radio))  //del from group
                    radio.Remove(this);
                m_groupName = null;
                if (value != null)
                    groupName = value;//loop
            } else {//原無群組的群組設置
                if (value != null) {
                    if (!_dic_radio.TryGetValue(value, out List<RadioBool> radio)) {//add new group
                        radio = new List<RadioBool>();
                        _dic_radio.Add(value, radio);
                    }
                    //add new rbool item
                    radio.Add(this);
                    m_groupName = value;
                }
            }
        }
    }

    /// <summary>RadioBool，組建一個Radio控制的群組Bool</summary>
    /// <param name="io">bool值</param>
    /// <param name="groupName">群組名稱(null=無群組控制)</param>
    /// <param name="resultAction">群組SwitchBool事件接收</param>
    public RadioBool(bool io, string groupName = null, ResultAction resultAction = null)
    {
        this.groupName = groupName;
        if (resultAction != null)
            this.resultAction += resultAction;
        this.io = io;
    }

    void SetBool(bool b)
    {
        if (m_io != b) {
            m_io = b;
            if (resultAction != null)
                resultAction(m_io);//群組SwitchBool事件發送
        }
    }

    /// <summary>RadioBoo的群組名稱列表</summary>
    public static List<string> GetGroupTableList() => _dic_radio.Select(s => s.Key).ToList();

    /// <summary>RadioBoo群組狀態</summary>
    public static string GetGroupStatus(string groupName)
    {
        if (_dic_radio.TryGetValue(groupName, out List<RadioBool> radio)) {
            if (radio != null) {
                return groupName + radio.Select(n => n.m_io).ToList().ToString<bool>();
            }
        }
        return "";
    }
}

/*************************************************
  * 名稱：Singleton
  * 作者：RyanHsu
  * 功能說明：單例模型
  * ***********************************************/
using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class Singleton
{
    protected static Dictionary<Type, object> _dic = new Dictionary<Type, object>();

    /// <summary> 抓取Instance </summary>
    /// <param name="autoCreat">如果未抓取到，是否自動創建實例</param>
    public static T GetInstance<T>() where T : Singleton
    {
        if (!_dic.ContainsKey(typeof(T))) _dic.Add(typeof(T), null);
        return (T)(_dic[typeof(T)] ??= Activator.CreateInstance<T>());
    }

    /// <summary> 抓取Instance，不會自動創建實例 </summary>
    public static bool TryGetInstance<T>(out T instance) where T : Singleton
    {
        instance = GetInstance<T>();
        return (instance != null);
    }

    public static int GetCount() => _dic.Count;
    public static bool ContainsKey<T>() => _dic.ContainsKey(typeof(T));
}

public abstract class Singleton<T> : Singleton, IDisposable where T : Singleton<T>
{
    public Singleton()
    {
        if (!_dic.ContainsKey(typeof(T))) _dic.Add(typeof(T), this);
    }

    /// <summary> 抓取Instance </summary>
    /// <param name="autoCreat">如果未抓取到，是否自動創建實例</param>
    public static T GetInstance(bool autoCreat = true) => GetInstance<T>();

    public static bool TryGetInstance(out T instance) => TryGetInstance<T>(out instance);

    public void Dispose()
    {
        if (_dic.ContainsKey(typeof(T))) _dic.Remove(typeof(T));
    }
}

public abstract class SingletonContain<T, K> : Singleton<T> where T : SingletonContain<T, K> where K : SingletonBehaviour<K>
{
    public K self;

    public virtual void OnAwake() { }
    public virtual void OnEnable() { }
    public virtual void OnDisable() { }
    public virtual void OnDestroy() { }
    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnFixedUpdate() { }
    public virtual void OnLateUpdate() { }

    public SingletonContain()
    {
        self = SingletonBehaviour<K>.GetInstance();
        self.onAwake += OnAwake;
        self.onEnable += OnEnable;
        self.onDisable += OnDisable;
        self.onDestroy += OnDestroy;
        self.onStart += OnStart;
        self.onUpdate += OnUpdate;
        self.onFixedUpdate += OnFixedUpdate;
        self.onLateUpdate += OnLateUpdate;
    }

}
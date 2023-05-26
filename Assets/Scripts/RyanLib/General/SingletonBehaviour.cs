/*************************************************
  * 名稱：SingletonBehaviour
  * 作者：RyanHsu
  * 功能說明：Behaviour單例模型
  * ***********************************************/
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public abstract class SingletonBehaviour : MonoBehaviour
{
    protected static Dictionary<Type, object> _dic = new Dictionary<Type, object>();

    /// <summary> 抓取Instance </summary>
    public static T GetInstance<T>() where T : SingletonBehaviour
    {
        if (!_dic.ContainsKey(typeof(T))) _dic.Add(typeof(T), FindObjectOfType<T>() ?? Activator.CreateInstance(typeof(T)));
        return (T)(_dic[typeof(T)] ??= FindObjectOfType<T>() ?? Activator.CreateInstance(typeof(T)));
    }

    /// <summary> 抓取Instance</summary>
    /// <param name="instance">out實例</param>
    /// <returns>是否無實例在場景中(被自動創建實例)</returns>
    public static bool TryGetInstance<T>(out T instance) where T : SingletonBehaviour
    {
        bool io = true;

        if (!_dic.ContainsKey(typeof(T))) _dic.Add(typeof(T), FindObjectOfType<T>() ?? CreateInstance());
        instance = (T)(_dic[typeof(T)] ??= FindObjectOfType<T>() ?? CreateInstance());

        T CreateInstance()
        {
            io = false;
            return Activator.CreateInstance(typeof(T)) as T;
        }

        return io;
    }

    public static void Clear() => _dic = new Dictionary<Type, object>();

    public static int GetCount() => _dic.Select(m => m.Value).Where(m => m != null).ToList().Count;
    public static List<object> GetValues() => _dic.Select(m => m.Value).Where(m => m != null).ToList();
    public static bool ContainsKey<T>() => _dic.ContainsKey(typeof(T)) && _dic[typeof(T)] != null;
}

/// <summary> 繼承MonoBehavior的Singleton </summary>
/// <typeparam name="T">Singleto的類別</typeparam>
public abstract class SingletonBehaviour<T> : SingletonBehaviour where T : SingletonBehaviour<T>
{
    public Action onAwake, onStart, onEnable, onDisable, onDestroy, onUpdate, onFixedUpdate, onLateUpdate;

    void Awake() { if (!_dic.ContainsKey(typeof(T))) _dic.Add(typeof(T), this); if (onAwake != null) onAwake.Invoke(); }
    void Start() { if (onStart != null) onStart.Invoke(); }
    void OnEnable() { if (onEnable != null) onEnable.Invoke(); }
    void OnDisable() { if (onDisable != null) onDisable.Invoke(); }
    void OnDestroy() { if (_dic.ContainsKey(typeof(T))) _dic.Remove(typeof(T)); if (onDestroy != null) onDestroy.Invoke(); onAwake = onStart = onEnable = onDisable = onDestroy = onUpdate = onFixedUpdate = onLateUpdate = null; }
    void Update() { if (onUpdate != null) onUpdate.Invoke(); }
    void FixedUpdate() { if (onFixedUpdate != null) onFixedUpdate.Invoke(); }
    void LateUpdate() { if (onLateUpdate != null) onLateUpdate.Invoke(); }

    /// <summary> 抓取Instance </summary>
    public static T GetInstance() => GetInstance<T>();

    public static K GetInterface<K>() where K : class => GetInstance<T>() as K;

    public static bool TryGetInstance(out T instance) => TryGetInstance<T>(out instance);

    public static bool TryGetInterface<K>(out K face) where K : class
    {
        face = GetInterface<K>();
        return (face != null);
    }

}
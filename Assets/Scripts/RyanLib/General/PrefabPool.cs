/*************************************************
  * 名稱：PrefabPool
  * 作者：RyanHsu
  * 功能說明：對向池管理
  * 測試資源 Test_PrefabPool
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using ExtensionMethods;
using System;
using UnityEngine.UI;

[SerializeField]
public interface IPrefabPool
{
    GameObject prefab { get; set; }

    /// <summary>物件計數</summary>
    int m_ItemCount { get; }

    /// <summary>回收池計數</summary>
    int m_PoolCount { get; }

    /// <summary>總計數</summary>
    int m_TotalCount { get; }

    /// <summary>回收所有Item</summary>
    void ClearAll();

    /// <summary>產生GameObject Item</summary>
    /// <param name="count">取用Pool個數</param>
    List<GameObject> PickItemByCount(int count);

    /// <summary>產生帶有某Class的Item</summary>
    /// <param name="count">取用Pool個數</param>
    List<T> PickItemByCount<T>(int count) where T : class;
}

public class PrefabPool : MonoBehaviour, IPrefabPool
{
    [Header("Prefab來源")] [CantBeNull] [SerializeField] GameObject m_Prefab;
    public GameObject prefab { get => m_Prefab; set => m_Prefab = value; }

    [Header("實例添加處")] [CantBeNull] public Transform m_Content;
    [Header("回收池存放處")] [CantBeNull] public Transform m_PoolParent;
    LayoutGroup m_LayoutGroup;//有LayoutGroup時，新增刪除完畢會刷新

    [Header("可Instantiate容量大小(0為無限大)")] [Min(0)] public int m_HoldCount = 0;//可Instantiate容量大小 (0為無限大)
    List<GameObject> m_Item { get { return transform.getAllChild().Select(m => m.gameObject).ToList(); } }//物件池
    List<GameObject> m_Pool { get { return m_PoolParent.getAllChild().Select(m => m.gameObject).ToList(); } }//回收池
    public int m_ItemCount { get { return transform.childCount; } }//Item計數
    public int m_PoolCount { get { return m_PoolParent.childCount; } }//回收池計數
    public int m_TotalCount { get { return m_ItemCount + m_PoolCount; } }//總計數

    public List<T> PickItemByCount<T>(int count) where T : class
    {
        List<T> item = new List<T>();
        ClearAll();
        for (int i = 1; i <= count; i++) {
            if (m_HoldCount != 0 && i > m_HoldCount) { Debug.Log($"因為HoldCount容量設定為({m_HoldCount})，PickItemByCount({count})已超過設定上限"); break; }

            if (typeof(T) == typeof(GameObject)) {
                item.Add(CreatItem() as T);
            }
            else {
                item.Add(CreatItem().GetComponent<T>());
            }
        }
        Reflash_LayoutGroup();
        return item;
    }

    public List<GameObject> PickItemByCount(int count) => PickItemByCount<GameObject>(count);

    /// <summary>取用Item</summary>
    public GameObject CreatItem()
    {
        GameObject item;
        if (m_PoolCount <= 0) {
            item = Instantiate(m_Prefab, m_Content);
            item.name = m_Prefab.name + "(Clone)";
        }
        else
            item = ReturnItem(m_Pool[0]);

        Reflash_LayoutGroup();
        return item;
    }

    GameObject ReturnItem(GameObject p)
    {
        p.transform.SetParent(m_Content, false);
        p.SetActive(true);
        return p;
    }

    /// <summary>Prefab放入回收池</summary>
    public void DestoryItem(GameObject p)
    {
        p.transform.SetParent(m_PoolParent, false);
        p.SetActive(false);
        Reflash_LayoutGroup();
    }

    public void ClearAll()
    {
        m_Item.ForEach(m => { if (m.name == m_Prefab.name + "(Clone)") DestoryItem(m); });

        Reflash_LayoutGroup();
    }

#if UNITY_EDITOR
    public void ClearPool()
    {
        m_Pool.ForEach(m => { if (m.name == m_Prefab.name + "(Clone)") DestroyImmediate(m); });

        Reflash_LayoutGroup();
    }
#endif

    void Reflash_LayoutGroup() { if (m_LayoutGroup) LayoutRebuilder.ForceRebuildLayoutImmediate(m_LayoutGroup.transform as RectTransform); }

#if UNITY_EDITOR
    public void CreatItemPrefab()
    {
        if (m_PoolCount <= 0) {
            var obj = UnityEditor.PrefabUtility.InstantiatePrefab(m_Prefab, m_Content);
            obj.name = m_Prefab.name + "(Clone)";
        }
        else
            ReturnItem(m_Pool[0]);
    }

    void OnValidate()
    {
        if (TryGetComponent(out LayoutGroup g)) m_LayoutGroup = g;
        if (m_Content == null) m_Content = transform;
    }

#endif

}

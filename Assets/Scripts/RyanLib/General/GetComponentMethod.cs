/*************************************************
  * 名稱：GetComponentMethod
  * 作者：RyanHsu
  * 功能說明：把GetComponent的運算包裝成內置函數取用，節省重覆Get運算的資源浪費
  * 使用說明：建立實例後，最少要執行過一次GetComponent，之後即可使用get()、tryGet()抓取com
  * ***********************************************/
using UnityEngine;
using System.Linq;
using ExtensionMethods;

/// <summary>把GetComponent的運算包裝成內置函數取用，節省重覆Get運算的資源浪費</summary>
public class GetComponentMethod<T> where T : Object
{
    T com = null;
    public T GetComponent(Component target) => com ? com : com = target.GetComponent<T>();
    public bool TryGetComponent(Component target, out T com) => com = GetComponent(target);
    public T GetComponentInParent(Component target) => com ? com : com = target.GetComponentInParent<T>();
    public bool TryGetComponentInParent(Component target, out T com) => com = GetComponentInParent(target);
    public T GetComponentInChildren(Component target) => com ? com : com = target.GetComponentInChildren<T>();
    public bool TryGetComponentInChildren(Component target, out T com) => com = GetComponentInChildren(target);
}

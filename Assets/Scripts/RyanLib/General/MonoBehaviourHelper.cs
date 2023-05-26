/*************************************************
  * 名稱：StartCoroutine Whitout MonoBehaviour
  * 作者：ryanhsu
  * 功能說明：讓非MonoBehaviour的程式也能支援使用StartCoroutine
  * 
  * 來源：https://blog.csdn.net/qq_39849535/article/details/111245922?spm=1001.2101.3001.6661.1&utm_medium=distribute.pc_relevant_t0.none-task-blog-2%7Edefault%7ECTRLIST%7Edefault-1.no_search_link&depth_1-utm_source=distribute.pc_relevant_t0.none-task-blog-2%7Edefault%7ECTRLIST%7Edefault-1.no_search_link
  * ***********************************************/

using System.Collections;
using UnityEngine;

#if UNITY_EDITOR
[ExecuteAlways]
#endif
public static class MonoBehaviourHelper
{
    /// <summary>啟動一個IEnumerator協程</summary>
    /// <param name="routine">傳入協程</param>
    /// <param name="persistent">DontDestroyOnLoad</param>
    /// <returns></returns>
    public static Coroutine StartCoroutine(IEnumerator routine, bool persistent = false)
    {
        GameObject go = new GameObject("Coroutine");
        Object.DontDestroyOnLoad(go);
        MonoBehaviourInstance MonoHelper = go.AddComponent<MonoBehaviourInstance>();
        return MonoHelper.DestroyWhenComplete(routine, persistent);
    }

    public class MonoBehaviourInstance : MonoBehaviour
    {
        public Coroutine DestroyWhenComplete(IEnumerator routine, bool persistent)
        {
            if (persistent) DontDestroyOnLoad(gameObject);
            return StartCoroutine(DestroyObjHandler(routine));
        }
        IEnumerator DestroyObjHandler(IEnumerator routine)
        {
            yield return StartCoroutine(routine);
#if UNITY_EDITOR
            DestroyImmediate(gameObject);
#else
            Destroy(this.gameObject);
#endif

        }
    }
}

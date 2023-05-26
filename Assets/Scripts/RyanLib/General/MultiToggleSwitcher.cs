/*************************************************
  * 名稱：ToggleIO
  * 作者：RyanHsu
  * 功能說明：多個UnityEvent的狀態切換組件。
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ExtensionMethods;
using UnityEngine.SceneManagement;
using UnityEditor;

//禁止多載         [DisallowMultipleComponent]
#if UNITY_EDITOR
using UnityEditor.SceneManagement;
//[ExecuteAlways]
#endif
//依賴項           [RequireComponent(typeof CLASS)]
public class MultiToggleSwitcher : MonoBehaviour
{
    [Header("功能說明")] [TextArea] public string m_Header;
    [Header("事件切換")] [DynamicRange(0, "m_ActionStatus")] public int m_Status;
    [Header("事件List")] public UnityEvent[] m_ActionStatus;

    DirtyInt m_Dirty = new DirtyInt();

    void Update()
    {
        if (m_Dirty.isDirty(m_Status)) {
            DoAction();
        }
    }

    public void DoAction(int index)
    {
        m_Status = index;
        DoAction();
    }

    public void DoAction()
    {
        m_ActionStatus[m_Status].Invoke();
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        if (!EditorApplication.isPlaying) {
            DoAction();
            Scene activeScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            EditorSceneManager.MarkSceneDirty(activeScene);
        }
    }
    //void OnDrawGizmos() { }
    //private void OnDrawGizmosSelected() { }
#endif

}

/*************************************************
  * 作者：RyanHsu
  * 功能說明：將此GameObject置入DontDestroyOnLoad，並且為Singleton，
  * 於SceneLoad後Awake判斷已存在=>移除GameObject
  * ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    static List<string> instance = new List<string>();

    void Awake()
    {
        if (!instance.Contains(name)) {
            Object.DontDestroyOnLoad(gameObject);
            instance.Add(name);
        } else {
            DestroyImmediate(gameObject, true);
        }
    }

    void Start()
    {
        
    }

}

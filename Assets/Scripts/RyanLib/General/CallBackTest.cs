/*************************************************
  * 名稱：CallBackTest
  * 作者：RyanHsu
  * 功能說明：
  * 注意：函數聲明必需為static
  * ***********************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallBackTest : MonoBehaviour
{
    // No1.子系統註冊回調
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)] static void RuntimeInitialize_SubsystemRegistration() => Debug.Log("/////////-SubsystemRegistration-/////////");

    // No2.所有程序集被加載和預加載資產被初始化後調用
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)] static void RuntimeInitialize_AfterAssembliesLoaded() => Debug.Log("/////////-AfterAssembliesLoaded-/////////");

    // No3.啟動畫面顯示之前調用
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)] static void RuntimeInitialize_BeforeSplashScreen() => Debug.Log("/////////-BeforeSplashScreen-/////////");

    // No4.場景加載前調用
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)] static void RuntimeInitialize_BeforeSceneLoad() => Debug.Log("/////////-BeforeSceneLoad-/////////");

    // No5.場景加載完畢調用 無參默認缺省值為AfterSceneLoad
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)] static void RuntimeInitialize_AfterSceneLoad() => Debug.Log("/////////-AfterSceneLoad-/////////");

}

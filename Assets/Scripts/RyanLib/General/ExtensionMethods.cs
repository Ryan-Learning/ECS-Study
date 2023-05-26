/*************************************************
  * 名稱：ExtensionMethods
  * 作者：RyanHsu
  * 功能說明：類別擴充 enum 、Array、List...
  * ***********************************************/

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text;
using System.Globalization;
using TMPro;

namespace ExtensionMethods
{
    public static class floatExtenstions
    {
        public static Vector3 ToVector3(this float value) => new Vector3(value, value, value);
        public static TweenerCore<float, float, FloatOptions> To(this float value, float endValue, float duration, Action<float> action)
        {
            return DOTween.To(() => value, x => value = x, endValue, duration).OnUpdate(() => action.Invoke(value)).OnComplete(() => action.Invoke(value));
        }
    }

    public static class intExtenstions
    {
        public static Vector3 ToVector3(this int value) => new Vector3(value, value, value);
    }

    public static class EnumExtenstions
    {
        /// <summary>擴充enum輸出欄位名稱的method</summary>
        /// <returns>回傳enum的欄位名稱string</returns>
        public static string ToDescription(this Enum value)
        {
            try {
                return value.GetType().GetRuntimeField(value.ToString())
                .GetCustomAttributes<System.ComponentModel.DescriptionAttribute>()
                .FirstOrDefault().Description ?? string.Empty;
            }
            catch (Exception) {
                Debug.LogWarning("Enum has no Description Attribute");
                return string.Empty;
            }
        }
        public static string ToDescription(this Enum value, string replaceOldChar, string replaceNewChar)
        {
            try {
                return value.GetType()
                .GetRuntimeField(value.ToString().Replace(replaceOldChar, replaceNewChar))
                .GetCustomAttributes<System.ComponentModel.DescriptionAttribute>()
                .FirstOrDefault().Description ?? string.Empty;
            }
            catch (Exception) {
                Debug.LogWarning("Enum has no Description Attribute");
                return string.Empty;
            }
        }

        public static void SetValue<TEnum>(this TEnum enumeration, int value) where TEnum : Enum
        {
            if (Enum.IsDefined(typeof(TEnum), value))
                enumeration = (TEnum)Enum.ToObject(typeof(TEnum), value);
            else
                Debug.Log("The Value '[{0}]' is not defined in the {1} Enum".WriteLine(value, typeof(TEnum)));
        }
    }

    public static class ArrayExtenstions
    {
        /// <summary>擴充Array類比List的Add method</summary>
        /// <typeparam name="T">Array的類別</typeparam>
        /// <param name="originArray">來源元素</param>
        /// <param name="element">增加的元素內容</param>
        public static void Add<T>(this T[] originArray, T element)
        {
            Array.Resize(ref originArray, originArray.Length + 1);
            originArray[originArray.Length - 1] = element;
        }

        /// <summary>擴充Array類比List的Del method</summary>
        /// <typeparam name="T">Array的類別</typeparam>
        /// <param name="originArray">來源元素</param>
        /// <param name="num">欲刪除的元素index編號</param>
        public static void Del<T>(this T[] originArray, int num)
        {
            originArray = originArray.Where<T>((val, index) => index != num).ToArray<T>();
        }

        /// <summary>將Array泛類T輸出為字串</summary>
        public static string ToString<T>(this T[] originArray)
        {
            string str = "[ ";
            int i = 0;
            foreach (T item in originArray) {
                str += str == "[ " ? "" : " , ";
                str += (i++) + ":";
                str += item.ToString();
            }
            str += " ]";
            return str;
        }

        /// <summary>ForEach</summary>
        public static void ForEach<T>(this T[] array, Action<T> action)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++) {
                action.Invoke(array[i]);
            }
        }

        /// <summary>帶編號的ForEach</summary>
        public static void ForEach<T>(this T[] array, Action<int, T> action)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++) {
                action.Invoke(i, array[i]);
            }
        }

        /// <summary>帶編號的ForEach，action會依index帶入queue</summary>
        public static void ForEach<T, T2>(this T[] array, Action<int, T, T2> action, params T2[] queue)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++) {
                action.Invoke(i, array[i], queue.CorrectValue(i));
            }
        }

        /// <summary>判斷index不會OutOfRange，且非Null</summary>
        public static bool CorrectIndex<T>(this T[] array, int index) => index >= array.Length || index < 0 ? false :
                (default(T) != null) ? true : (array[index] != null);  //如果T支援Null為空值，才去判斷內容是否為Null，否則一率true

        /// <summary>以index安全的取出值，如OutOfRange，支援Null的會回傳Null，否則將以 T 類型的 Default回傳</summary>
        public static T CorrectValue<T>(this T[] array, int index) => CorrectIndex<T>(array, index) ? array[index] : default;

    }

    public static class ListExtenstions
    {
        /// <summary>將List泛類T輸出為字串</summary>
        public static string ToString<T>(this List<T> list)
        {
            string str = "[ ";
            int i = 0;
            foreach (T item in list) {
                str += str == "[ " ? "" : " , ";
                str += (i++) + ":";
                str += item.ToString();
            }
            str += " ]";
            return str;
        }

        /// <summary>帶編號的ForEach</summary>
        public static void ForEach<T>(this List<T> list, Action<int, T> action)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++) {
                action.Invoke(i, list[i]);
            }
        }

        /// <summary>帶編號的ForEach，action會依index帶入queue</summary>
        public static void ForEach<T, T2>(this List<T> list, Action<int, T, T2> action, params T2[] queue)
        {
            int count = list.Count;
            for (int i = 0; i < count; i++) {
                action.Invoke(i, list[i], queue.CorrectValue(i));
            }
        }

        /// <summary>判斷index不會OutOfRange，且非Null</summary>
        public static bool CorrectIndex<T>(this List<T> list, int index) => index >= list.Count || index < 0 ? false :
                (default(T) != null) ? true : (list[index] != null);  //如果T支援Null為空值，才去判斷內容是否為Null，否則一率true

        /// <summary>以index安全的取出值，如OutOfRange，支援Null的會回傳Null，否則將以 T 類型的 Default回傳</summary>
        public static T CorrectValue<T>(this List<T> list, int index) => CorrectIndex<T>(list, index) ? list[index] : default;
    }

    public static class HashtableExtenstions
    {
        /// <summary>移除HashTabelByValue值</summary>
        public static void RemoveByValue(this Hashtable hash, object value)
        {
            hash.ForEach((_key, _value) =>
            {
                if (_value == value) hash.Remove(_key);
            });
        }

        /// <summary>ForEach</summary>
        /// /// <param name="action"> Action<key,value> </param>
        public static void ForEach(this Hashtable hash, Action<object, object> action)
        {
            foreach (object key in hash) {
                action.Invoke(key, hash[key]);
            }
        }
        /// <summary>帶編號的ForEach</summary>
        /// <param name="action"> Action<index,key,value> </param>
        public static void ForEach(this Hashtable hash, Action<int, object, object> action)
        {
            int count = 0;
            foreach (object key in hash) {
                action.Invoke(count++, key, hash[key]);
            }
        }
    }

    public static class stringExtenstions
    {
        /// <summary>將string以#分割成FileName與FilePath</summary>
        public static string ToFileName(this string str, string split = "#") => str.Split(split)[1];

        /// <summary>將string以#分割成FileName與FilePath</summary>
        public static string ToFilePath(this string str, string split = "#") => str.Split(split)[0];

        /// <summary>將string分割成Vector</summary>
        ///<param name="split">分割符號</param><param name="format">xyz排序</param>
        public static Vector3 ToVector(this string str, string split = ",", enum_Vector format = enum_Vector.xyz)
        {
            float[] sp = str.Split(split).Select(m => float.TryParse(m, out float f) ? f : 0f).ToArray();
            return sp.Length switch
            {
                2 => format switch
                {
                    enum_Vector.yx or enum_Vector.yxz => new Vector2(sp[1], sp[0]),
                    _ => new Vector2(sp[0], sp[1]),
                },
                3 => format switch
                {
                    enum_Vector.zxy => new Vector3(sp[2], sp[0], sp[1]),
                    enum_Vector.zyx => new Vector3(sp[2], sp[1], sp[0]),
                    enum_Vector.xzy => new Vector3(sp[0], sp[2], sp[1]),
                    enum_Vector.yzx => new Vector3(sp[1], sp[2], sp[0]),
                    enum_Vector.yxz or enum_Vector.yx => new Vector3(sp[1], sp[0], sp[2]),
                    _ => new Vector3(sp[0], sp[1], sp[2]),
                },
                _ => Vector3.zero,
            };
        }

        /// <summary>JsonConvert.DeserializeObject</summary>
        public static bool TryToJsonObj<T>(this string str, out T obj) where T : new()
        {
            obj = JsonConvert.DeserializeObject<T>(str) ?? new T();
            return obj != null;
        }

        /// <summary>JsonConvert.DeserializeObject</summary>
        public static T ToJsonObj<T>(this string str) where T : new()
        {
            T obj = JsonConvert.DeserializeObject<T>(str);
            return obj == null ? new T() : obj;
        }

        /// <summary> 將String加密為Escape的Base64 </summary>
        public static string ToEnBase64URL(this string str)
        {
            try {
                str = UnityWebRequest.EscapeURL(str, Encoding.UTF8).Replace("+", "%20");
                byte[] bytesToEncode = Encoding.UTF8.GetBytes(str);
                return Convert.ToBase64String(bytesToEncode) ?? "";
            }
            catch (Exception) {
                return "";
            }
        }

        /// <summary> 將Escape的Base64解密回String </summary>
        public static string ToDeBase64URL(this string str)
        {
            try {
                byte[] base64EncodedBytes = Convert.FromBase64String(str);
                str = Encoding.UTF8.GetString(base64EncodedBytes);
                return UnityWebRequest.UnEscapeURL(str.Replace("%20", "+"), Encoding.UTF8) ?? "";
            }
            catch (Exception) {
                return "";
            }
        }

        /// <summary>文字輸出float運算</summary><param name="d">小數位數</param>
        public static float ToFloat(this string str, int d = 3) => String.IsNullOrWhiteSpace(str) ? 0 : MathF.Round(float.Parse(str, NumberStyles.Float, CultureInfo.InvariantCulture), d);
        /// <summary>文字輸出Int運算</summary>
        public static int ToInt(this string str) => String.IsNullOrWhiteSpace(str) ? 0 : Mathf.RoundToInt(float.Parse(str, NumberStyles.Float, CultureInfo.InvariantCulture));
        /// <summary>[非複寫]以Console.WriteLine規則取代{0}-{N}的變數params</summary>
        public static string WriteLine(this string str, params object[] pars) { pars.ForEach((i, m) => { str = str.Replace("{" + i + "}", m.ToString()); }); return str; }
    }

    public static class ObjectExtenstions
    {
        /// <summary> ForEach 一個 Object 內的指定 T 類型 Property</summary>
        /// <typeparam name="T">指定列舉類型</typeparam>
        public static void ForEachProperty<T>(this object obj, Action<string, T> action)
        {
            foreach (var property in obj.GetType().GetFields()) {
                if (property.FieldType == typeof(T)) action.Invoke(property.Name, (T)property.GetValue(obj));
            }
        }
        /// <summary> ForEach 一個 Object 內的指定 T 類型 Property</summary>
        /// <typeparam name="T">指定列舉類型</typeparam>
        public static void ForEachProperty<T>(this object obj, Action<int, string, T> action)
        {
            int index = 0;
            foreach (var property in obj.GetType().GetFields()) {
                if (property.FieldType == typeof(T)) action.Invoke(index++, property.Name, (T)property.GetValue(obj));
            }
        }

        /// <summary>Get 一個 Object 內的指定 T 類型 List<Property> </summary>
        /// <typeparam name="T">指定列舉類型</typeparam>
        public static List<T> GetProperty<T>(this object obj)
        {
            List<T> list_bind = new List<T>();
            foreach (var property in obj.GetType().GetFields()) {
                if (property.FieldType == typeof(T)) {
                    list_bind.Add((T)property.GetValue(obj));
                }
            }
            return list_bind;
        }
    }

    public static class GenericsExtenstions
    {
        public static bool TryToJsonString<T>(this T obj, out string str)
        {
            str = JsonConvert.SerializeObject(obj);
            return str != "";
        }
        public static string ToJsonString<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj) ?? "";
        }
    }

    public static class BehaviorExtenstions
    {
        /// <summary>擴充behavior的Auto turn enable</summary>
        public static bool enableTrigger(this Behaviour behavior)
        {
            return behavior.enabled = !behavior.enabled;
        }

        /// <summary>擴充MonoBehaviour執行StartCoroutine的DelayCall</summary>
        public static Coroutine DelayCall(this MonoBehaviour mono, float delaySeconds, Action complete, Action<float> progress = null)
        {
            return mono.StartCoroutine(ToDo());
            IEnumerator ToDo()
            {
                float second = delaySeconds;
                while (second > 0) {//false就跳離
                    yield return new WaitForSeconds(0.1f);
                    second -= 0.1f;
                    if (progress != null) progress.Invoke(second);
                }
                complete.Invoke();
            }
        }

    }

    public static class CanvasGroupExtenstions
    {
        /// <summary>擴充DoTween支援CanvasGroup DOAlpha的簡易輸入補間</summary>
        public static DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> DOAlpha(this CanvasGroup target, float endValue, float duration)
        {
            return DG.Tweening.DOTween.To(() => target.alpha, value => target.alpha = value, endValue, duration);
        }
    }

    public static class RectTransformExtenstions
    {
        public static RectTransform ifNull(this RectTransform tf, Func<RectTransform> fun)
        {
            if (tf) return tf; else tf = fun.Invoke(); return tf;
        }
        public static List<RectTransform> getAllChild(this RectTransform tf)
        {
            List<RectTransform> tf_list = new List<RectTransform>();
            foreach (RectTransform t in tf) {
                tf_list.Add(t);
            }
            return tf_list;
        }

        public static Vector2 RWD_Scale(this RectTransform tf)
        {
            return tf.GetComponentInParent<RectTransform>().lossyScale;
        }

        /// <summary>擴充RectTransform: 無論何種Anchors型式都能正確取座標WorldToLocal</summary>
        public static Vector2 WorldToAnchorsPosition(this RectTransform tf, Vector3 v3)
        {
            return v3 - tf.TransformPoint(-tf.anchoredPosition);
        }
        /// <summary>擴充RectTransform: 無論何種Anchors型式都能正確取座標WorldToLocal</summary>
        public static Vector3 WorldToAnchorsPosition3D(this RectTransform tf, Vector3 v3)
        {
            return v3 - tf.TransformPoint(-tf.anchoredPosition);
        }
        /// <summary>擴充RectTransform: 無論何種Anchors型式都能正確取座標LocalToWorld</summary>
        public static Vector2 AnchorsToWorldPosition(this RectTransform tf, Vector3 v3)
        {
            return v3 + tf.TransformPoint(-tf.anchoredPosition);
        }
        /// <summary>擴充RectTransform: 無論何種Anchors型式都能正確取座標LocalToWorld</summary>
        public static Vector3 AnchorsToWorldPosition3D(this RectTransform tf, Vector3 v3)
        {
            return v3 + tf.TransformPoint(-tf.anchoredPosition);
        }

        /// <summary>擴充DoTween支援RectTransform DOMove的簡易輸入補間</summary>
        public static DG.Tweening.Core.TweenerCore<Vector2, Vector2, DG.Tweening.Plugins.Options.VectorOptions> DOMove(this RectTransform target, Vector2 endValue, float duration)
        {
            return DG.Tweening.DOTween.To(() => target.anchoredPosition, value => target.anchoredPosition = value, endValue, duration);
        }
    }

    public static class TransformExtenstions
    {
        /// <summary>Transform轉型別為RectTransform</summary>
        public static RectTransform rect(this Transform tf)
        {
            return (RectTransform)tf;
        }

        /// <summary>擴充globalScale值的輸入</summary>
        public static void SetGlobalScale(this Transform transform, Vector3 globalScale)
        {
            transform.localScale = Vector3.one;
            transform.localScale = new Vector3(globalScale.x / transform.lossyScale.x, globalScale.y / transform.lossyScale.y, globalScale.z / transform.lossyScale.z);
        }

        /// <summary>傳單層子項Child Transform List</summary>
        public static bool TryGetChild(this Transform tf, int index, out Transform child)
        {
            child = null;
            if (tf.childCount <= index) return false;
            child = tf.GetChild(index);
            return child != null ? true : false;
        }

        /// <summary>傳單層子項Child Transform List</summary>
        public static List<Transform> getAllChild(this Transform tf)
        {
            List<Transform> tf_list = new List<Transform>();
            foreach (Transform t in tf) {
                tf_list.Add(t);
            }
            return tf_list;
        }

        /// <summary>傳深度層級子項Child Transform List</summary>
        public static List<Transform> getDeepChild(this Transform tf)
        {
            return tf.GetComponentsInChildren<Transform>().ToList() ?? new List<Transform>();
        }

        /// <summary>傳深度父級Transform List</summary>
        public static List<Transform> getAllParent(this Transform tf)
        {
            return tf.GetComponentsInParent<Transform>().ToList() ?? new List<Transform>();
        }

        /// <summary>ForEach運算回傳單層子項Child Transform</summary>
        public static void ForEach(this Transform transform, Action<Transform> action)
        {
            foreach (Transform tf in transform) {
                action.Invoke(tf);
            }
        }

        /// <summary>ForEach運算回傳 "帶編號index" 單層子項Child Transform</summary>
        public static void ForEach(this Transform transform, Action<int, Transform> action)
        {
            foreach (Transform tf in transform) {
                action.Invoke(tf.GetSiblingIndex(), tf);
            }
        }
    }

    public static class GameObjectExtenstions
    {
        /// <summary>傳單層子項Child GameObject List</summary>
        public static List<GameObject> getAllChild(this GameObject go)
        {
            List<GameObject> go_list = new List<GameObject>();
            foreach (Transform tf in go.transform) {
                go_list.Add(tf.gameObject);
            }
            return go_list;
        }
        /// <summary>傳深度層級子項Child GameObject List</summary>
        public static List<GameObject> getDeepChild(this GameObject go)
        {
            return go.transform.GetComponentsInChildren<Transform>().Select(m => m.gameObject).ToList() ?? new List<GameObject>();
        }
        /// <summary>傳深度父級GameObject List</summary>
        public static List<GameObject> getAllParent(this GameObject go)
        {
            return go.transform.GetComponentsInParent<Transform>().Select(m => m.gameObject).ToList() ?? new List<GameObject>();
        }
        /// <summary>ForEach運算回傳單層子項Child GameObject</summary>
        public static void ForEach(this GameObject gameObject, Action<GameObject> action)
        {
            foreach (Transform tf in gameObject.transform) {
                action.Invoke(tf.gameObject);
            }
        }

        /// <summary>ForEach運算回傳 "帶編號index" 單層子項Child GameObject</summary>
        public static void ForEach(this GameObject gameObject, Action<int, GameObject> action)
        {
            foreach (Transform tf in gameObject.transform) {
                action.Invoke(tf.GetSiblingIndex(), tf.gameObject);
            }
        }
    }

    public static class VectorExtenstions
    {
        public static Vector3 SetSame(this Vector3 v3, float value)
        {
            v3.Set(value, value, value);
            return v3;
        }
        /// <summary>[非複寫]value取代XY</summary>
        public static Vector2 SetSame(this Vector2 v2, float value)
        {
            v2.Set(value, value);
            return v2;
        }
        /// <summary>[非複寫]value取代X</summary>
        public static Vector3 SetX(this Vector3 v3, float newV)
        {
            v3.Set(newV, v3.y, v3.z);
            return v3;
        }
        /// <summary>[非複寫]value取代Y</summary>
        public static Vector3 SetY(this Vector3 v3, float newV)
        {
            v3.Set(v3.x, newV, v3.z);
            return v3;
        }
        /// <summary>[非複寫]value取代Z</summary>
        public static Vector3 SetZ(this Vector3 v3, float newV)
        {
            v3.Set(v3.x, v3.y, newV);
            return v3;
        }
        /// <summary>[非複寫]value取代X</summary>
        public static Vector2 SetX(this Vector2 v2, float newV)
        {
            v2.Set(newV, v2.y);
            return v2;
        }
        /// <summary>[非複寫]value取代Y</summary>
        public static Vector2 SetY(this Vector2 v2, float newV)
        {
            v2.Set(v2.x, newV);
            return v2;
        }

        ///<summary>將Vector3轉成以,分割的String</summary>
        ///<param name="format">xyz排序轉譯</param><param name="d">小數位數</param>
        public static string ToVectorString(this Vector3 v, enum_FormatV3 format = enum_FormatV3.xyz, int d = 2)
        {
            string strD = (d <= 0) ? "0" : "0.";
            for (int i = 0; i < d; i++) { strD += "0"; }
            switch (format) {
                case enum_FormatV3.zxy: return v.z.ToString(strD) + "," + v.x.ToString(strD) + "," + v.y.ToString(strD);
                case enum_FormatV3.zyx: return v.z.ToString(strD) + "," + v.y.ToString(strD) + "," + v.x.ToString(strD);
                case enum_FormatV3.yxz: return v.y.ToString(strD) + "," + v.x.ToString(strD) + "," + v.z.ToString(strD);
                case enum_FormatV3.yzx: return v.y.ToString(strD) + "," + v.z.ToString(strD) + "," + v.x.ToString(strD);
                case enum_FormatV3.xzy: return v.x.ToString(strD) + "," + v.z.ToString(strD) + "," + v.y.ToString(strD);
                case enum_FormatV3.xyz: default: return v.x.ToString(strD) + "," + v.y.ToString(strD) + "," + v.z.ToString(strD);
            }
        }
        ///<summary>將Vector2轉成以,分割的String</summary>
        ///<param name="format">xz排序轉譯</param><param name="d">小數位數</param>
        public static string ToVectorString(this Vector2 v, enum_FormatV2 format = enum_FormatV2.xy, int d = 2)
        {
            string strD = (d <= 0) ? "0" : "0.";
            for (int i = 0; i < d; i++) { strD += "0"; }
            switch (format) {
                case enum_FormatV2.yx: return v.y.ToString(strD) + "," + v.x.ToString(strD);
                case enum_FormatV2.xy: default: return v.x.ToString(strD) + "," + v.y.ToString(strD);
            }
        }

        /// <summary>轉成Vector2</summary>
        public static Vector2 ToVector2(this Vector3 v3)
        {
            return v3;
        }

        /// <summary>修正 NaN或Infinity錯誤 ，以0取代</summary>
        public static Vector2 FixNanFinite(this Vector2 v2)
        {
            if (float.IsFinite(v2.x) && float.IsFinite(v2.y)) {
                return v2;
            }
            else {
                Vector2 newV2 = new Vector2(float.IsFinite(v2.x) ? v2.x : 0, float.IsFinite(v2.y) ? v2.y : 0);
                return newV2;
            }
        }
    }

    public static class DictionaryExtenstions
    {
        /// <summary>帶編號的ForEach</summary>
        public static void ForEach<TKey, TValue>(this Dictionary<TKey, TValue> dic, Action<int, TKey, TValue> action)
        {
            int count = dic.Count;
            int i = 0;
            foreach (var m in dic) {
                action.Invoke(i++, m.Key, m.Value);
            }
        }
    }

    public static class TMP_InputFieldExtenstions
    {
        /// <summary>色彩漸變擴充</summary>
        public static TweenerCore<Color, Color, ColorOptions> DoColor(this TMP_InputField field, Color color1, Color color2, float duration, Action OnCpmplete = null)
        {
            ColorBlock block = field.colors;
            Color NormalColor = block.normalColor;
            Color ChangeColor = new Color();
            return DOTween.To(() => color1, c => ChangeColor = c, color2, duration).OnUpdate(() =>
            {
                block.normalColor = ChangeColor;
                field.colors = block;
            });
        }

        public static void DoColor_PingPong(this TMP_InputField field, Color color1, Color color2, float duration, int PingPongCount = 2, Action OnCpmplete = null)
        {
            Sequence seq = DOTween.Sequence();
            ColorBlock block = field.colors;
            Color NormalColor = block.normalColor;
            float average = duration * 0.5f / PingPongCount;
            for (int i = 0; i < PingPongCount; i++) {
                seq.Append(DoColor(field, color1, color2, average));
                seq.Append(DoColor(field, color2, color1, average));
            }
            seq.Play();
        }
    }

    public enum enum_FormatV2 { xy, yx }
    public enum enum_FormatV3 { xyz, xzy, yxz, yzx, zxy, zyx }
    public enum enum_Vector
    {
        xy, yx,
        xyz, xzy, yxz, yzx, zxy, zyx,
    }
}

/*************************************************
  * 名稱：ObjectPool
  * 作者：RyanHsu
  * 功能說明：簡易物件池
  * ***********************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
public class ObjectPool<T> {
    private Queue<T> _pool = new Queue<T> ();
    private Func<T> _creatFun;

    public ObjectPool (Func<T> creatFun) {
        if (creatFun == null) throw new ArgumentException ("建構方程式不可null");
        _creatFun = creatFun;
    }

    public T GetInstance () {
        if (_pool.Count != 0) return _pool.Dequeue ();
        else return _creatFun ();
    }

    public void RecycleObject (T item) {
        _pool.Enqueue (item);
    }

}
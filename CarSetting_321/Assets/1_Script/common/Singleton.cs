using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 继承mono的父类
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class DDOLSingleton<T> : MonoBehaviour where T: DDOLSingleton<T>{
    protected static object obj = new object();
    protected static T _instance;

    public static T Instance
    {
        get {
            lock(obj)
            {
                if (_instance==null)
                {
                    GameObject go = GameObject.Find("DDOLsingleton");
                    if(go==null)
                    {
                        go = new GameObject("DDOLsingleton");
                        DontDestroyOnLoad(go);
                    }

                    _instance = go.GetComponent<T>();
                    if(_instance == null)
                    {
                        _instance = go.AddComponent<T>();
                    }
                }
                return _instance;
            }
          

        }
    }
}

public abstract class Singleton<T> where T : class,new()
{
    protected static object obj = new object();
    protected static T _instance;
    public static T Instance
    {
        get
        {
            lock (obj)
            {
                if (_instance == null)
                {
                    _instance = new T();
                }
                return _instance;
            }

        }
    }


}
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour where T : Component
{
    //单件子类实例
    public static T _instance;

    /// <summary>
    ///     获得单例，查询场景中是否有该种类型，如果有存储静态遍历，如果没有，构建一个带有这个component的gameobject
    ///     这种单例的GameObject直接挂接在bootroot节点下，在场景中的生命周期和游戏周期相同
    ///     创建这个单例的模块必须通过DestroyInstance自行管理单例的生命周期
    /// </summary>
    /// <returns>返回单件实例</returns>
    public static T GetInstance()
    {
        if (_instance == null)
        {
            Type theType = typeof(T);

            _instance = (T)FindObjectOfType(theType);

            if (_instance == null)
            {
                var go = new GameObject(typeof(T).Name);
                _instance = go.AddComponent<T>();

                //挂接到BootObj下
                GameObject bootObj = GameObject.Find("BootObj");
                if (bootObj == null)
                {
                    bootObj = new GameObject("BootObj");
                    DontDestroyOnLoad(bootObj);
                }
                go.transform.SetParent(bootObj.transform);
            }
        }
        return _instance;
    }

    /// <summary>
    ///     删除单例，这种继承关系的单例生命周期应该由模块显示管理 
    /// </summary>
    public static void DestroyInstance()
    {
        if (_instance != null)
        {
            Destroy(_instance.gameObject);
        }
        _instance = null;
    }

    /// <summary>
    ///     Awake消息，确保单例的唯一性
    /// </summary>

    protected virtual void Awake()
    {
        if (_instance != null && _instance.gameObject != gameObject)
        {
            if (Application.isPlaying)
            {
                Destroy(gameObject);
            }
            else
            {
                DestroyImmediate(gameObject);//UNITY_EDITOR
            }
        }
        else if (_instance == null)
        {
            _instance = GetComponent<T>();
        }

        DontDestroyOnLoad(gameObject);

        Init();
    }

    /// <summary>
    ///     OnDestroy消息，确保单例会随着GameObject销毁
    /// </summary>
    protected virtual void OnDestroy()
    {
        if (_instance != null && _instance.gameObject == gameObject)
        {
            _instance = null;
        }
    }

    public virtual void DestroySelf()
    {
        _instance = null;
        Destroy(gameObject);
    }

    public static bool HasInstance()
    {
        return _instance != null;
    }

    protected virtual void Init()
    {

    }
}

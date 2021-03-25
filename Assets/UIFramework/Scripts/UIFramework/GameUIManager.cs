using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoSingleton<GameUIManager>
{
    //UI列表缓存
    static Dictionary<Type, ScreenBase> mTypeScreens = new Dictionary<Type, ScreenBase>();

    public GameObject uiRoot;
    public GameObject poolRoot  // 缓存节点
    {
        get;
        private set;
    }

    public int mUIOpenOrder = 0;// UI打开时的Order值 用来标识界面层级顺序
    //uiCamera
    Camera uiCamera;
    public Camera UICamera { get => uiCamera; }

    protected override void Init()
    {
        //初始化UI根节点
        uiRoot = Instantiate(Resources.Load<GameObject>("UIRoot"), transform);
        uiCamera = uiRoot.GetComponent<Canvas>().worldCamera;

        //初始化UI缓存池
        poolRoot = new GameObject("UIPoolRoot");
        poolRoot.transform.SetParent(transform);

        Canvas canvas = poolRoot.AddComponent<Canvas>();
        canvas.enabled = false;
    }

    /// <summary>
    /// 打开UI
    /// </summary>
    /// <param name="type"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public ScreenBase OpenUI(Type type, UIOpenScreenParameterBase param = null)
    {
        ScreenBase sb = GetUI(type);
        mUIOpenOrder++;

        //如果已有界面，则不执行任何操作
        if (sb != null)
        {
            if (sb.CtrlBase != null && !sb.CtrlBase.ctrlCanvas.enabled)
            {
                sb.CtrlBase.ctrlCanvas.enabled = true;
            }

            HanderLogic(sb);
            return sb;
        }
        sb = (ScreenBase)Activator.CreateInstance(type, param);

        mTypeScreens.Add(type, sb);
        sb.SetOpenOrder(mUIOpenOrder);// 设置打开序号

        HanderLogic(sb);

        return sb;
    }

    /// <summary>
    /// UI外部调用的获取接口
    /// </summary>
    public ScreenBase GetUI(Type type)
    {
        if (!typeof(ScreenBase).IsAssignableFrom(type)) return default;
        ScreenBase sb = null;
        if (mTypeScreens.TryGetValue(type, out sb))
            return sb;
        return null;
    }

    /// <summary>
    /// UI外部调用的获取接口
    /// </summary>
    public TScreen GetUI<TScreen>() where TScreen : ScreenBase
    {
        Type type = typeof(TScreen);
        ScreenBase sb = null;

        if (mTypeScreens.TryGetValue(type, out sb))
        {
            return (TScreen)sb;
        }
        return null;
    }

    public bool CloseUI(Type type)
    {
        ScreenBase sb = GetUI(type);
        if (sb != null)
        {
            if (type == typeof(ScreenBase))
                return false;
            else
                sb.OnClose();
            return true;
        }
        return false;
    }

    public void CloseAllUI()
    {
        //销毁会从容器中删除 不能用正常遍历方式
        List<Type> keys = new List<Type>(mTypeScreens.Keys);
        foreach (var k in keys)
        {
            if (k == typeof(ScreenBase))
            {
                continue;
            }
            if (mTypeScreens.ContainsKey(k))
                mTypeScreens[k].OnClose();
        }
    }

    /// <summary>
    /// UI创建时候自动处理的UI打开处理 一般不要手动调用
    /// </summary>
    public void AddUI(ScreenBase sBase)
    {
        sBase.mPanelRoot.transform.SetParent(GetUIRootTransform());

        sBase.mPanelRoot.transform.localPosition = Vector3.zero;
        sBase.mPanelRoot.transform.localScale = Vector3.one;
        sBase.mPanelRoot.transform.localRotation = Quaternion.identity;
        sBase.mPanelRoot.name = sBase.mPanelRoot.name.Replace("(Clone)", "");
    }

    /// <summary>
    /// UI移除时自动处理的接口 一般不要手动调用
    /// </summary>
    public void RemoveUI(ScreenBase sBase)
    {
        if (mTypeScreens.ContainsKey(sBase.GetType()))
            mTypeScreens.Remove(sBase.GetType());
        sBase.Dispose();
        HanderLogic(sBase);
    }

    //处理以下额外逻辑
    void HanderLogic(ScreenBase sb)
    {
        //处理最上层界面
        if (sb.CtrlBase.mHideOtherScreenWhenThisOnTop)
        {
            ProcessUIOnTop();
        }

        //处理货币栏变化
        ChangeMoneyType();
    }

    /// <summary>
    /// 处理最上层界面逻辑
    /// </summary>
    List<ScreenBase> sortTemp = new List<ScreenBase>();
    void ProcessUIOnTop()
    {
        sortTemp.Clear();
        foreach (var s in mTypeScreens.Values)
        {
            sortTemp.Add(s);
        }
        //排序，按照层级 高->低 的顺序
        sortTemp.Sort((a, b) =>
            {
                if (a.mSortingLayer == b.mSortingLayer)
                {
                    return b.mOpenOrder.CompareTo(a.mOpenOrder);
                }
                return b.mSortingLayer.CompareTo(a.mSortingLayer);
            });

        //先找到i的一个控制的UI层
        int index = 0;
        for (int i = 0; i < sortTemp.Count; i++)
        {
            var tempC = sortTemp[i];
            if (tempC.CtrlBase.mHideOtherScreenWhenThisOnTop)
            {
                //找到第一个需要被隐藏的界面 隐藏就好
                tempC.CtrlBase.ctrlCanvas.enabled = true;
                index = i;// 因为是一个有序的List 所以找到第一个需要控制的界面之后记录序号，然后从它开始遍历即可
                break;
            }
        }

        //如果没有找到 可能的情况是就是关闭了最上层界面 所以现在最上层的应该是空的
        if (index == 0)
        {
            for (int i = 0; i < sortTemp.Count; i++)
            {
                var tempC = sortTemp[i];
                // 找到第一个需要被隐藏的界面 隐藏就好
                if (!tempC.CtrlBase.ctrlCanvas.enabled)
                {
                    tempC.CtrlBase.ctrlCanvas.enabled = true;
                    index = 1;// 因为是一个有序的List 所以找到第一个需要控制的界面之后记录序号，然后从它开始遍历即可
                    break;
                }
            }
        }

        //找到下面需要隐藏的
        for (int i = index + 1; i < sortTemp.Count; i++)
        {
            var tempC = sortTemp[i];
            if (!tempC.CtrlBase.mAlwaysShow)
            {
                // 找到需要被隐藏的界面 隐藏就好
                tempC.CtrlBase.ctrlCanvas.enabled = false;
            }
        }
    }

    /// <summary>
    /// 处理货币栏变化逻辑
    /// </summary>
    void ChangeMoneyType()
    {
        sortTemp.Clear();
        foreach (var s in mTypeScreens.Values)
        {
            sortTemp.Add(s);
        }
        //排序 按照层级 高->低 的顺序
        sortTemp.Sort(
            (a, b) =>
            {
                if (a.mSortingLayer == b.mSortingLayer)
                {
                    return b.mOpenOrder.CompareTo(a.mOpenOrder);
                }
                return b.mSortingLayer.CompareTo(a.mSortingLayer);
            });

        // 找到第一个关心货币栏的
        for (int i = 0; i < sortTemp.Count; i++)
        {
            if (sortTemp[i].CtrlBase.mBCareAboutMoney)
            {
                EventManager.OnMoneyTypeChange.BroadCastEvent(sortTemp[i].CtrlBase.MoneyTypes);
                break;
            }
        }
    }


    //返回登陆界面时，重置常驻UI的状态
    public void Reset()
    {

    }

    #region 通用API
    //获取UIRoot节点
    public Transform GetUIRootTransform()
    {
        return transform;
    }

    public Camera GetUICamera()
    {
        return uiCamera;
    }
    #endregion
}

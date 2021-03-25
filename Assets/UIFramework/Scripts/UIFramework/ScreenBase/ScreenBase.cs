using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBase
{
    public GameObject mPanelRoot = null;
    public string mStrUIName = "";
    protected UICtrlBase mCtrlBase;

    public int mOpenOrder = 0;// 界面打开顺序
    public int mSortingLayer = 0;// 界面层级

    //界面打开的传入参数
    protected UIOpenScreenParameterBase mOpenParam;

    public UICtrlBase CtrlBase { get => mCtrlBase; }

    public ScreenBase(string UIName, UIOpenScreenParameterBase param = null)
    {
        StartLoad(UIName, param);
    }

    //开始加载UI资源
    public virtual void StartLoad(string UIName, UIOpenScreenParameterBase param = null)
    {
        mStrUIName = UIName;
        mOpenParam = param;
        ResourcesManager.GetInstance().LoadAsset<GameObject>(UIName, PanelLoadComplete);
    }

    //资源加载完成
    void PanelLoadComplete(GameObject ao)
    {
        mPanelRoot = Object.Instantiate(ao, GameUIManager.GetInstance().GetUIRootTransform());
        //获取控件对象
        mCtrlBase = mPanelRoot.GetComponent<UICtrlBase>();

        //更新层级信息
        UpdateLayoutLevel();

        //调用加载成功方法
        OnLoadSuccess();

        //添加到控制层
        GameUIManager.GetInstance().AddUI(this);
        Debug.Log("资源加载完成");
    }

    // 脚本处理完成
    virtual protected void OnLoadSuccess()
    {
        if (mCtrlBase.m_UseMask)
        {
            MaskScreenManager.GetInstance().Show(this);
            Debug.Log("打开遮罩");
        }
    }

    //当UI关闭时
    virtual public void OnClose()
    {
        GameUIManager.GetInstance().RemoveUI(this);
    }

    // 界面打开的时候会根据选项自动给界面加上背景遮罩层，点击遮罩层传递的事件，可重写
    virtual public void OnClickMaskArea()
    {
        OnClose();
    }

    //释放
    public virtual void Dispose()
    {
        GameObject.Destroy(mPanelRoot);
    }

    //设置渲染顺序
    public void SetOpenOrder(int openOrder)
    {
        mOpenOrder = openOrder;
        if (mCtrlBase != null && mCtrlBase.ctrlCanvas != null)
        {
            mCtrlBase.ctrlCanvas.sortingOrder = openOrder;
        }
    }

    //更新UI的层级
    private void UpdateLayoutLevel()
    {
        var camera = GameUIManager.GetInstance().GetUICamera();
        if (camera != null)
        {
            Debug.Log("Camera");
            mCtrlBase.ctrlCanvas.worldCamera = camera;
        }

        mCtrlBase.ctrlCanvas.pixelPerfect = true;
        mCtrlBase.ctrlCanvas.overrideSorting = true;
        mCtrlBase.ctrlCanvas.sortingLayerID = (int)mCtrlBase.sceenPriority;

        mSortingLayer = (int)mCtrlBase.sceenPriority;
        mCtrlBase.ctrlCanvas.sortingOrder = mOpenOrder;
    }

}

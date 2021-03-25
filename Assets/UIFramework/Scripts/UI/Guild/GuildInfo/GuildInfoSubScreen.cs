using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuildInfoSubScreen : SubScreenBase
{
    GuildInfoSubCtrl mCtrl;// 公会信息控件

    public GuildInfoSubScreen(GuildInfoSubCtrl subCtrl) : base(subCtrl) { }

    protected override void Init()
    {
        mCtrl = mCtrlBase as GuildInfoSubCtrl;
        mCtrl.btnClose.onClick.AddListener(OnCloseClick);
        mCtrl.btnJumpTask.onClick.AddListener(OnTaskClick);
    }

    void OnCloseClick()
    {
        GameUIManager.GetInstance().CloseUI(typeof(GuildScreen));
    }

    void OnTaskClick()
    {
        GameUIManager.GetInstance().OpenUI(typeof(TaskScreen));
    }
}

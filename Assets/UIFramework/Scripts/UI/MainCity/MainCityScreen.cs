using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCityScreenParam : UIOpenScreenParameterBase
{

}

public class MainCityScreen : ScreenBase
{
    MainCityCtrl mCtrl;

    public MainCityScreen(UIOpenScreenParameterBase param = null) : base(UIConst.UIMainCity, param)
    {

    }

    protected override void OnLoadSuccess()
    {
        base.OnLoadSuccess();
        mCtrl = mCtrlBase as MainCityCtrl;

        mCtrl.btnGuild.onClick.AddListener(OnGuildClik);

        mCtrl.txtLv.text = 20.ToString();
    }

    /// <summary>
    /// 点击了公会按钮
    /// </summary>
    void OnGuildClik()
    {
        GameUIManager.GetInstance().OpenUI(typeof(GuildScreen));
    }
}

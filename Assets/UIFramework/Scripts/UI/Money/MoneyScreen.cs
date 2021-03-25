using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyScreen : ScreenBase
{
    MoneyCtrl mCtrl;

    public MoneyScreen(UIOpenScreenParameterBase param = null) : base(UIConst.UIMoney, param) { }

    protected override void OnLoadSuccess()
    {
        base.OnLoadSuccess();
        mCtrl = mCtrlBase as MoneyCtrl;

        //监听货币栏变化事件
        mCtrl.AutoRelease(EventManager.OnMoneyTypeChange.Subscribe(OnMoneyTypeChange));

    }

    void OnMoneyTypeChange(EUICareAboutMoneyType[] types)
    {
        mCtrl.btnSilver.gameObject.SetActive(false);
        mCtrl.btnGold.gameObject.SetActive(false);
        mCtrl.btnStrength.gameObject.SetActive(false);
        mCtrl.btnGem.gameObject.SetActive(false);

        foreach (var t in types)
        {
            if (t == EUICareAboutMoneyType.Silver)
            {
                mCtrl.btnSilver.gameObject.SetActive(true);
            }
            else if (t == EUICareAboutMoneyType.Gold)
            {
                mCtrl.btnGold.gameObject.SetActive(true);
            }
            else if (t == EUICareAboutMoneyType.Strength)
            {
                mCtrl.btnStrength.gameObject.SetActive(true);
            }
            else if (t == EUICareAboutMoneyType.Gem)
            {
                mCtrl.btnGem.gameObject.SetActive(true);
            }
        }

    }
}

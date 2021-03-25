using UnityEngine;

public class GuildScreen : ScreenBase
{

    GuildCtrl mCtrl;

    GuildCreateSubScreen mSubCreate;
    GuildInfoSubScreen mSubInfo;

    public GuildScreen(UIOpenScreenParameterBase param = null) : base(UIConst.UIGuild) { }

    protected override void OnLoadSuccess()
    {
        base.OnLoadSuccess();
        mCtrl = mCtrlBase as GuildCtrl;

        //监听公会创建成功事件
        mCtrl.AutoRelease(EventManager.OnGuildCreated.Subscribe(OnGuildCreated));

        //有公会就会打开公会详情，没有就打开创建界面
        bool bHaveGuild = PlayerData.GetInstance().HaveGuild();
        //处理子界面的显示隐藏
        mCtrl.subInfo.gameObject.SetActive(bHaveGuild);
        mCtrl.subCreate.gameObject.SetActive(!bHaveGuild);

        //处理子界面逻辑初始化
        if (bHaveGuild)
        {
            mSubInfo = new GuildInfoSubScreen(mCtrl.subInfo);
        }
        else
        {
            mSubCreate = new GuildCreateSubScreen(mCtrl.subCreate);
        }
    }

    void OnGuildCreated(bool bCreated)
    {
        mCtrl.subInfo.gameObject.SetActive(bCreated);
        mCtrl.subCreate.gameObject.SetActive(!bCreated);

        if (bCreated)
            mSubInfo = new GuildInfoSubScreen(mCtrl.subInfo);
        else
            mSubCreate = new GuildCreateSubScreen(mCtrl.subCreate);
    }
}

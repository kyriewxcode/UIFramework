using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    protected static PlayerData instance;

    public static PlayerData GetInstance()
    {
        if (instance == null)
        {
            instance = new PlayerData();
        }
        return instance;
    }

    /// <summary>
    /// 是否拥有公会
    /// </summary>
    bool mBHasGuild = false;
    public bool HaveGuild()
    {
        return mBHasGuild;
    }

    /// <summary>
    /// 设置是否拥有公会
    /// </summary>
    /// <param name="bHaveGuild"></param>
    public void SetHaveGuild(bool bHaveGuild)
    {
        mBHasGuild = bHaveGuild;
        EventManager.OnGuildCreated.BroadCastEvent(mBHasGuild);
    }
}

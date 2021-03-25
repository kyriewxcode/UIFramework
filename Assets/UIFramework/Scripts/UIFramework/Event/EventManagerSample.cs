using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManagerSample
{
    protected static EventManagerSample instance;

    public static EventManagerSample GetInstance()
    {
        if (instance == null)
        {
            instance = new EventManagerSample();
        }
        return instance;
    }

    public delegate void EventCallBack(object param);//事件回调函数

    //事件字典
    Dictionary<int, List<EventCallBack>> mDictEvent = new Dictionary<int, List<EventCallBack>>();

    //增加监听
    public void AddEvent(int eventId, EventCallBack callBack)
    {
        if (!mDictEvent.ContainsKey(eventId))
        {
            mDictEvent.Add(eventId, new List<EventCallBack>());
        }

        if (!mDictEvent[eventId].Contains(callBack))
        {
            mDictEvent[eventId].Add(callBack);
        }
        else
        {
            Debug.LogWarning("Repeat Add Event CallBack, EventId = " + eventId + ",CallBack = " + callBack.ToString());
        }
    }

    //删除监听
    public void DelEvent(int eventId, EventCallBack callBack)
    {
        if (!mDictEvent.ContainsKey(eventId))
        {
            return;
        }
        if (!mDictEvent[eventId].Contains(callBack))
        {
            return;
        }
        mDictEvent[eventId].Remove(callBack);

        //如果回调全都被移除了，则移除key
        if (mDictEvent[eventId].Count < 1)
        {
            mDictEvent.Remove(eventId);
        }
    }

    //通知事件
    public void NotifyEvent(int eventId, object param)
    {
        if (mDictEvent.ContainsKey(eventId))
        {
            foreach (var callback in mDictEvent[eventId])
            {
                callback(param);
            }
        }
    }
}

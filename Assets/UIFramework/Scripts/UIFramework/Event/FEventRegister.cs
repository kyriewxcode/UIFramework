﻿using System;
public interface IRelease
{
    void Release();
}

public abstract class FEventRegisterBase
{
    protected Delegate _delegate;

    /// <summary>
    /// 添加监听
    /// </summary>
    protected void _AddEventHandler(Delegate d)
    {
        _delegate = Delegate.Combine(_delegate, d);
    }

    /// <summary>
    /// 移除监听
    /// </summary>
    protected void _RemoveEventHandler(Delegate d)
    {
        _delegate = Delegate.RemoveAll(_delegate, d);
    }

    /// <summary>
    /// 订阅监听，订阅的监听不用remove，需要执行返回接口的Release。
    /// 该功能可以方便统一管理，实现自动移除监听。
    /// </summary>
    protected IRelease _Subscribe(Delegate cb)
    {
        _AddEventHandler(cb);
        return new HandlerRemover(this, cb);
    }
    class HandlerRemover : IRelease
    {
        FEventRegisterBase _source;
        Delegate _value;

        public HandlerRemover(FEventRegisterBase source, Delegate value)
        {
            _source = source;
            _value = value;
        }

        void IRelease.Release()
        {
            _source._RemoveEventHandler(_value);
        }
    }
}

public abstract class FEventRegister : FEventRegisterBase
{
    public void AddEventHandler(Action cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action cb)
    {
        _RemoveEventHandler(cb);
    }

    //订阅事件
    public IRelease Subscribe(Action cb)
    {
        return _Subscribe(cb);
    }

    /// <summary>
    /// 注册器不应该拥有广播事件的功能
    /// 把这个方法提取到这里是为了减少重复代码，方便多个子类复用
    /// </summary>
    protected void _BroadCastEvent()
    {
        if (_delegate != null)
        {
            (_delegate as Action)();
        }
    }
}

//一个参数
public abstract class FEventRegister<T0> : FEventRegisterBase
{
    public void AddEventHandler(Action<T0> cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action<T0> cb)
    {
        _RemoveEventHandler(cb);
    }
    //定阅事件
    public IRelease Subscribe(Action<T0> cb)
    {
        return _Subscribe(cb);
    }
    protected void _BroadCastEvent(T0 arg0)
    {
        if (_delegate != null)
        {
            (_delegate as Action<T0>)(arg0);
        }
    }
}

//两个参数
public abstract class FEventRegister<T0, T1> : FEventRegisterBase
{

    public void AddEventHandler(Action<T0, T1> cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action<T0, T1> cb)
    {
        _RemoveEventHandler(cb);
    }

    //定阅事件
    public IRelease Subscribe(Action<T0, T1> cb)
    {
        return _Subscribe(cb);
    }

    protected void _BroadCastEvent(T0 arg0, T1 arg1)
    {
        if (_delegate != null)
        {
            (_delegate as Action<T0, T1>)(arg0, arg1);
        }
    }
}

//三个参数
public abstract class FEventRegister<T0, T1, T2> : FEventRegisterBase
{
    public void AddEventHandler(Action<T0, T1, T2> cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action<T0, T1, T2> cb)
    {
        _RemoveEventHandler(cb);
    }

    //定阅事件
    public IRelease Subscribe(Action<T0, T1, T2> cb)
    {
        return _Subscribe(cb);
    }

    protected void _BroadCastEvent(T0 arg0, T1 arg1, T2 arg2)
    {
        if (_delegate != null)
        {
            (_delegate as Action<T0, T1, T2>)(arg0, arg1, arg2);
        }
    }
}

//四个参数
public abstract class FEventRegister<T0, T1, T2, T3> : FEventRegisterBase
{
    public void AddEventHandler(Action<T0, T1, T2, T3> cb)
    {
        _AddEventHandler(cb);
    }
    public void RemoveEventHandler(Action<T0, T1, T2, T3> cb)
    {
        _RemoveEventHandler(cb);
    }

    //定阅事件
    public IRelease Subscribe(Action<T0, T1, T2, T3> cb)
    {
        return _Subscribe(cb);
    }

    protected void _BroadCastEvent(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        if (_delegate != null)
        {
            (_delegate as Action<T0, T1, T2, T3>)(arg0, arg1, arg2, arg3);
        }
    }
}

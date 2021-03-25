
/// <summary>
/// 没参数
/// </summary>
public class FEvent : FEventRegister
{
    public void BroadCastEvent()
    {
        _BroadCastEvent();
    }
}

/// <summary>
/// 一个参数
/// </summary>
public class FEvent<T0> : FEventRegister<T0>
{
    public void BroadCastEvent(T0 arg0)
    {
        _BroadCastEvent(arg0);
    }
}


/// <summary>
/// 两个参数
/// </summary>
public class FEvent<T0, T1> : FEventRegister<T0, T1>
{
    public void BroadCastEvent(T0 arg0, T1 arg1)
    {
        _BroadCastEvent(arg0, arg1);
    }
}


/// <summary>
/// 三个参数
/// </summary>
public class FEvent<T0, T1, T2> : FEventRegister<T0, T1, T2>
{
    public void BroadCastEvent(T0 arg0, T1 arg1, T2 arg2)
    {
        _BroadCastEvent(arg0, arg1, arg2);
    }
}

/// <summary>
/// 四个参数
/// </summary>
public class FEvent<T0, T1, T2, T3> : FEventRegister<T0, T1, T2, T3>
{
    public void BroadCastEvent(T0 arg0, T1 arg1, T2 arg2, T3 arg3)
    {
        _BroadCastEvent(arg0, arg1, arg2, arg3);
    }
}

public class SubScreenBase
{
    protected UISubCtrlBase mCtrlBase;

    public UISubCtrlBase CtrlBase { get { return mCtrlBase; } }

    public SubScreenBase(UISubCtrlBase ctrlBase)
    {
        mCtrlBase = ctrlBase;
        Init();
    }

    protected virtual void Init()
    {

    }

    public virtual void Dispose()
    {

    }
}

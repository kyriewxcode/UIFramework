using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIFEventAutoRelease : MonoBehaviour
{
    protected List<IRelease> releaseIDList = new List<IRelease>();

    protected virtual void OnDestroy()
    {
        RemoveAllListener();
    }

    #region Auto Release Listener
    public void AutoRelease(IRelease release)
    {
        releaseIDList.Add(release);
    }

    private void RemoveAllListener()
    {
        foreach(var releaseID in releaseIDList)
        {
            releaseID.Release();
        }
    }
    
    #endregion
}
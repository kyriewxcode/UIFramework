using System;
using UnityEngine;

public class ResourcesManager : MonoSingleton<ResourcesManager>
{
    [HideInInspector]
    public Canvas ctrCanvas;

    public void LoadAsset<T>(string address, Action<GameObject> action)
    {
        var go = Resources.Load<GameObject>(address);
        if (action != null)
        {
            action?.Invoke(go);
        }
    }
}

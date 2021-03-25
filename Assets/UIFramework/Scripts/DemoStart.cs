using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoStart : MonoBehaviour
{
    void Start()
    {
        GameUIManager.GetInstance().OpenUI(typeof(MainCityScreen));
        GameUIManager.GetInstance().OpenUI(typeof(MoneyScreen));
    }
}

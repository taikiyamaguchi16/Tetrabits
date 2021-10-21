using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHolder : MonoBehaviour
{
    //保有しているオブジェクト
    private GameObject ownObj;

    //オブジェクトを保有しているかどうか
    public GameObject GetBattery()
    {
        return ownObj;
    }
    //
    public void SetBattery(GameObject _battery)
    {
        ownObj = _battery;
    }
}

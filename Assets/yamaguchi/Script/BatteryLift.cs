using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryLift : MonoBehaviour
{
    [SerializeField]
    Transform startPos;
    [SerializeField]
    Transform endPos;

    [SerializeField]
    GameObject carryBattery;
    public void SetBatteryLiftPos(float _time,float _maxTime)
    {
        float rate = Mathf.Clamp01(_time / _maxTime);   // 割合計算
        // 移動・回転
        carryBattery.transform.position = Vector3.Lerp(startPos.position, endPos.position, rate);
    }
}

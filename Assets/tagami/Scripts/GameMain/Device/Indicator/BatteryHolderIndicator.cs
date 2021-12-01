using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryHolderIndicator : MonoBehaviour
{
    [SerializeField] EmissionIndicator emissionIndicator;
    [SerializeField] BatteryHolder batteryHolder;

    // Update is called once per frame
    void Update()
    {
        //電池残量で操作する
        if (batteryHolder.GetBatterylevel() > 0)
        {
            emissionIndicator.SetColor(EmissionIndicator.ColorType.Using);
        }
        else
        {
            emissionIndicator.SetColor(EmissionIndicator.ColorType.Unusable);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLevelBar : MonoBehaviour
{
    [Header("Holder")]
    [SerializeField] BatteryHolder batteryHolder;
    [Header("Slider")]
    [SerializeField] BatteryBarSlider slider;
   

    // Update is called once per frame
    void FixedUpdate()
    {
        slider.value01 = batteryHolder.GetBatterylevel() / 100;
    }
}

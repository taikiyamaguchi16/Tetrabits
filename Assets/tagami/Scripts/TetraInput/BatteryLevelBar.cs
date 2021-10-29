using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BatteryLevelBar : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] BatteryHolder batteryHolder;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = batteryHolder.GetBatterylevel();
       // Debug.Log(batteryHolder.GetBatterylevel());
    }
}

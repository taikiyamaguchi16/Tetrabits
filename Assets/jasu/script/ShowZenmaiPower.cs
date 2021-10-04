using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowZenmaiPower : MonoBehaviour
{
    [SerializeField]
    Zenmai zenmai;

    [SerializeField]
    Slider slider;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = zenmai.maxZenmaiPower;
        slider.value = zenmai.maxZenmaiPower;
    }

    private void LateUpdate()
    {
        slider.value = zenmai.zenmaiPower;
    }
}

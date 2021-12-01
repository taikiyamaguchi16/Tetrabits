using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialBatteryLamp : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] string batteryHolderName;
    public BatteryHolder batteryHolder { private set; get; }

    [Header("Control Target")]
    [SerializeField] RawImage controlImage;

    // Start is called before the first frame update
    void Start()
    {
        var obj = GameObject.Find(batteryHolderName);
        if (obj)
        {
            batteryHolder = obj.GetComponent<BatteryHolder>();
        }
        else
        {
            Debug.LogError("BatteryHolderを見つけることができませんでした");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (batteryHolder.GetBatterylevel() > 0)
        {
            controlImage.color = Color.cyan;
        }
        else
        {
            controlImage.color = Color.red;
        }
    }
}

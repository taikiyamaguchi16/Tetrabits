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
    [SerializeField] RawImage colorChangeImage;
    [SerializeField] GameObject textureChangeImageOn;
    [SerializeField] GameObject textureChangeImageOff;

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
        if (batteryHolder && batteryHolder.GetBatterylevel() > 0)
        {
            textureChangeImageOn.SetActive(true);
            textureChangeImageOff.SetActive(false);
            colorChangeImage.color = Color.cyan;
        }
        else
        {
            textureChangeImageOn.SetActive(false);
            textureChangeImageOff.SetActive(true);
            colorChangeImage.color = Color.red;
        }
    }
}

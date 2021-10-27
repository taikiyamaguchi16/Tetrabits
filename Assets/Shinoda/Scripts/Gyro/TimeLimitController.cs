using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeLimitController : MonoBehaviour
{
    [SerializeField, Tooltip("チェック入れたらスリップのやつ")] bool isSlip;
    [SerializeField, Tooltip("スリップの間隔")] float slipRecast;
    [SerializeField,Tooltip("制限時間")] float limit;
    [SerializeField, Tooltip("ダメージ量")] float damage;

    Text timeText;

    float timeCount = 0f;
    float slipCount = 0f;

    // Start is called before the first frame update
    void Start()
    {
        timeText = transform.Find("TimeText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        timeCount += Time.deltaTime;
        slipCount += Time.deltaTime;
        if(isSlip)
        {
            if (timeCount > limit && slipCount > slipRecast)
            {
                MonitorManager.DealDamageToMonitor(damage);
                slipCount = 0;
            }
        }
        else
        {
            if (timeCount > limit)
            {
                MonitorManager.DealDamageToMonitor(damage);
                timeCount = 0;
            }
        }

        timeText.text = (limit - timeCount).ToString("f0");
    }
}

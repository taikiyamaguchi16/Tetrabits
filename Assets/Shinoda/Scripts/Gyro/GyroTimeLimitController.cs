using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;

public class GyroTimeLimitController : MonoBehaviour
{
    [SerializeField, Tooltip("チェック入れたらスリップのやつ")] bool isSlip;
    [SerializeField, Tooltip("制限時間")] float limit;
    [SerializeField, Tooltip("ダメージ量")] string damage = "small";
    [SerializeField, Tooltip("制限時間の強調表示")] int emphasis = 5;

    Transform timeTransform;
    Text timeText;

    float timeCount;
    int remainingTime;
    int beforeTime;
    bool damaging = false;
    bool timeStop = false;

    // Start is called before the first frame update
    void Start()
    {
        timeTransform = transform.Find("TimeText");
        timeText = timeTransform.GetComponent<Text>();
        timeCount = limit;
        remainingTime = (int)timeCount;
        beforeTime = remainingTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!damaging) timeCount -= Time.deltaTime;
        if (timeStop) remainingTime = 0;
        else remainingTime = (int)timeCount;

        if (timeCount < 0)
        {
            timeStop = true;

            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor(damage);
            }

            timeCount = limit;
            DamageAnimation();
        }

        if (remainingTime == emphasis && remainingTime != beforeTime)
        {
            EmphasisAnimation();
        }

        timeText.text = remainingTime.ToString("f0");
        beforeTime = remainingTime;
    }

    //void TimeLimitAnimation()
    //{
    //    if (remainingTime <= 5)
    //    {
    //        timeText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
    //        if (remainingTime != beforeTime) timeTransform.DOScale(1.3f, .5f).SetEase(Ease.Linear).OnComplete(() =>
    //        {
    //            timeTransform.DOScale(1f, .5f).SetEase(Ease.Linear);
    //        });
    //    }
    //    else
    //    {
    //        timeText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    //    }
    //}

    void EmphasisAnimation()
    {
        timeTransform.DOScale(1.5f, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            timeTransform.DOScale(1f, .5f).SetEase(Ease.Linear);
        });
    }

    void DamageAnimation()
    {
        damaging = true;
        timeText.color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        timeTransform.DOScale(1.5f, .5f).SetEase(Ease.Linear).OnComplete(() =>
        {
            timeTransform.DOScale(1f, .5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                timeText.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                if (!isSlip) timeStop = false;
                damaging = false;
            });
        });
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;

public class GyroTimeLimitController : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")] float time;
    [SerializeField, Tooltip("ダメージ量")] string damage = "large";

    [SerializeField] SceneObject thisScene = null;
    [SerializeField] SceneObject nextScene = null;
    [SerializeField] bool finalStage = false;
    bool loadable = true;

    [SerializeField] GameObject g;
    Text gText;
    [SerializeField] GameObject o;
    Text oText;
    [SerializeField] GameObject a;
    Text aText;
    [SerializeField] GameObject l;
    Text lText;

    Transform timeTransform;
    Text timeText;

    [SerializeField] GameObject missPanel;
    RectTransform panelTransform;

    int remainingTime;
    bool isGoal = false;

    // Start is called before the first frame update
    void Start()
    {
        timeTransform = transform.Find("TimeText");
        timeText = timeTransform.GetComponent<Text>();
        remainingTime = (int)time;

        gText = g.GetComponent<Text>();
        oText = o.GetComponent<Text>();
        aText = a.GetComponent<Text>();
        lText = l.GetComponent<Text>();
        panelTransform = missPanel.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGoal)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;

            remainingTime = (int)time;
            timeText.text = remainingTime.ToString("f0");
        }

        if (remainingTime == 0)
        {
            TimeUpAnimation();
        }
    }

    public void GoalAnimation()
    {
        isGoal = true;
        gText.DOColor(new Color(255, 255, 255, 255), 1).SetEase(Ease.Linear);
        oText.DOColor(new Color(255, 255, 255, 255), 1).SetDelay(.2f).SetEase(Ease.Linear);
        aText.DOColor(new Color(255, 255, 255, 255), 1).SetDelay(.4f).SetEase(Ease.Linear);
        lText.DOColor(new Color(255, 255, 255, 255), 1).SetDelay(.6f).SetEase(Ease.Linear).OnComplete(() =>
           {
               if (finalStage && loadable)
               {
                   GameInGameUtil.StopGameInGameTimer("gyro");
                   GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                   loadable = false;
               }
               else if (PhotonNetwork.IsMasterClient && loadable)
               {
                   GameInGameUtil.SwitchGameInGameScene(nextScene);
                   loadable = false;
               }
           });
    }

    public void TimeUpAnimation()
    {
        panelTransform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor(damage);
                MonitorManager.CallAddNumDebrisInGameMainStage();
                GameInGameUtil.SwitchGameInGameScene(thisScene);
            }
        });
    }
}
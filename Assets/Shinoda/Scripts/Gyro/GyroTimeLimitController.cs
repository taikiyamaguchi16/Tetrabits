using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;

public class GyroTimeLimitController : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")] float time;

    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip clearSE;

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
        timeTransform = GameObject.Find("TimeText").transform;
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
        if (!isGoal)
        {
            isGoal = true;
            SimpleAudioManager.PlayBGMCrossFade(BGM, 1, 0);
            SimpleAudioManager.PlayOneShot(clearSE);
            gText.DOColor(new Color(255, 255, 255, 255), 1.5f).SetEase(Ease.InQuint);
            oText.DOColor(new Color(255, 255, 255, 255), 1.5f).SetDelay(.3f).SetEase(Ease.InQuint);
            aText.DOColor(new Color(255, 255, 255, 255), 1.5f).SetDelay(.6f).SetEase(Ease.InQuint);
            lText.DOColor(new Color(255, 255, 255, 255), 1.5f).SetDelay(.9f).SetEase(Ease.InQuint).OnComplete(() =>
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
    }

    public void TimeUpAnimation()
    {
        panelTransform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.CallAddNumDebrisInGameMainStage();
                GameInGameUtil.SwitchGameInGameScene(thisScene);
            }
        });
    }
}
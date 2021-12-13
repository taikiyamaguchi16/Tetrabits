using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Photon.Pun;

public class JumpTimeController : MonoBehaviour
{
    [SerializeField, Tooltip("制限時間")] float time;
    [SerializeField, Tooltip("ダメージ量")] string damage = "large";

    [SerializeField] AudioClip goalSE;
    [SerializeField] SceneObject thisScene = null;
    [SerializeField] SceneObject nextScene = null;
    [SerializeField] bool finalStage = false;
    bool loadable = true;

    [SerializeField] GameObject g;
    RectTransform gTransform;
    [SerializeField] GameObject o;
    RectTransform oTransform;
    [SerializeField] GameObject a;
    RectTransform aTransform;
    [SerializeField] GameObject l;
    RectTransform lTransform;

    [SerializeField] GameObject missPanel;
    RectTransform panelTransform;

    Transform timeTransform;
    Text timeText;
    int remainingTime;
    bool isGoal = false;

    // Start is called before the first frame update
    void Start()
    {
        timeTransform = GameObject.Find("Time").transform;
        timeText = timeTransform.GetComponent<Text>();
        remainingTime = (int)time;

        gTransform = g.GetComponent<RectTransform>();
        oTransform = o.GetComponent<RectTransform>();
        aTransform = a.GetComponent<RectTransform>();
        lTransform = l.GetComponent<RectTransform>();
        panelTransform = missPanel.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGoal)
        {
            time -= Time.deltaTime;
            if (time < 0) time = 0;
            else if (time > 999) time = 999;

            remainingTime = (int)time;
            timeText.text = remainingTime.ToString("d3");
        }

        if (remainingTime == 0)
        {
            TimeUpAnimation();
        }
    }

    public void GoalAnimation()
    {
        isGoal = true;
        SimpleAudioManager.PlayOneShot(goalSE);
        gTransform.DOScale(new Vector3(1, 1, 1), 1.5f).SetEase(Ease.Linear);
        oTransform.DOScale(new Vector3(1, 1, 1), 1.5f).SetDelay(.3f).SetEase(Ease.Linear);
        aTransform.DOScale(new Vector3(1, 1, 1), 1.5f).SetDelay(.6f).SetEase(Ease.Linear);
        lTransform.DOScale(new Vector3(1, 1, 1), 1.5f).SetDelay(.9f).SetEase(Ease.Linear).OnComplete(() =>
           {
               if (finalStage && loadable)
               {
                   GameInGameUtil.StopGameInGameTimer("jump");
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
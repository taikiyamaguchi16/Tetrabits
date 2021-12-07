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

    [SerializeField] SceneObject thisScene = null;
    [SerializeField] SceneObject nextScene = null;
    bool loadable = true;

    [SerializeField] GameObject g;
    RectTransform gTransform;
    [SerializeField] GameObject o;
    RectTransform oTransform;
    [SerializeField] GameObject a;
    RectTransform aTransform;
    [SerializeField] GameObject l;
    RectTransform lTransform;

    Transform timeTransform;
    Text timeText;
    int remainingTime;
    bool isGoal = false;

    // Start is called before the first frame update
    void Start()
    {
        timeTransform = transform.Find("Time");
        timeText = timeTransform.GetComponent<Text>();
        remainingTime = (int)time;

        gTransform = g.GetComponent<RectTransform>();
        oTransform = o.GetComponent<RectTransform>();
        aTransform = a.GetComponent<RectTransform>();
        lTransform = l.GetComponent<RectTransform>();
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
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor(damage);
                GameInGameUtil.SwitchGameInGameScene(thisScene);
            }
        }
    }

    public void GoalAnimation()
    {
        isGoal = true;
        g.SetActive(true);
        o.SetActive(true);
        a.SetActive(true);
        l.SetActive(true);
        gTransform.DOScale(new Vector3(1, 1, 1), 1).SetEase(Ease.Linear);
        oTransform.DOScale(new Vector3(1,1,1),1).SetDelay(.2f).SetEase(Ease.Linear);
        aTransform.DOScale(new Vector3(1,1,1),1).SetDelay(.4f).SetEase(Ease.Linear);
        lTransform.DOScale(new Vector3(1,1,1),1).SetDelay(.6f).SetEase(Ease.Linear).OnComplete(() =>
        {
            if (nextScene == null) GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
            else if (PhotonNetwork.IsMasterClient && loadable)
            {
                GameInGameUtil.SwitchGameInGameScene(nextScene);
                loadable = false;
            }
        });
    }
}
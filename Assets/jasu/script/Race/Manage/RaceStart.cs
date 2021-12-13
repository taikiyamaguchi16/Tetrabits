using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStart : MonoBehaviour
{
    [SerializeField]
    BikeCtrlWhenStartAndGoal[] bikeCtrlStartGoals;

    [SerializeField]
    CountDown countDown;

    bool started = false;

    public bool GetStarted() { return started; }

    [SerializeField]
    bool firstStage = false;

    [SerializeField]
    AudioClip bgm;

    // Start is called before the first frame update
    void Start()
    {
        if (firstStage)
        {
            GameInGameUtil.StartGameInGameTimer("race");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown.isTimeOut && !started)
        {
            started = true;
            SimpleAudioManager.PlayBGMOverride(bgm);
            foreach (var bikeCtrlStartGoal in bikeCtrlStartGoals)
            {
                bikeCtrlStartGoal.SetActiveBeforeStart(true);
            }
            //Debug.Log("スタート");
        }
    }
}

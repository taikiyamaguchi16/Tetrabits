using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RaceFinish : MonoBehaviour
{
    [SerializeField]
    SceneObject nextStageScene = null;

    [SerializeField]
    SceneObject nowStageScene = null;

    [SerializeField]
    RaceManager raceManager;

    [SerializeField]
    RacerInfo playerInfo;

    [SerializeField]
    GameObject textObjWhenWin;

    [SerializeField]
    GameObject textObjWhenLose;

    bool taskWhenGoaled = false;

    [SerializeField]
    float standBytimeSeconds = 3f;

    float standByTimer = 0f;

    bool winFlag = false;

    [SerializeField]
    bool finalStage = false;

    // Update is called once per frame
    void Update()
    {
        if (taskWhenGoaled)
        {
            standByTimer += Time.deltaTime;
            if(standByTimer > standBytimeSeconds)
            {
                standByTimer = 0f;
                if (winFlag)
                {
                    if (finalStage)
                    {
                        GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                    }
                    else if (nextStageScene != null)
                    {
                        GameInGameUtil.SwitchGameInGameScene(nextStageScene);
                    }
                }
                else
                {
                    if(nowStageScene != null)
                    {
                        GameInGameUtil.SwitchGameInGameScene(nowStageScene);
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        // ゴール時のみ
        if (playerInfo.lapCounter.goaled && !taskWhenGoaled)   
        {
            raceManager.RankingCalculation();

            taskWhenGoaled = true;
            if (playerInfo.ranking == 1)
            {
                textObjWhenWin.SetActive(true);
                winFlag = true;
            }
            else
            {
                textObjWhenLose.SetActive(true);
            }
        }
    }
}

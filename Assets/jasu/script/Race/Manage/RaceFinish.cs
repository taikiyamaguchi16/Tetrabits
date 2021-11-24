using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceFinish : MonoBehaviour
{
    [SerializeField]
    SceneObject nextStageScene = null;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (taskWhenGoaled)
        {
            standByTimer += Time.deltaTime;
            if(standByTimer > standBytimeSeconds)
            {
                standByTimer = 0f;
                if(nextStageScene != null)
                {
                    GameInGameUtil.SwitchGameInGameScene(nextStageScene);
                }
            }
        }
        else if (playerInfo.lapCounter.goaled && !taskWhenGoaled)   // ゴール時のみ
        {
            taskWhenGoaled = true;
            if(playerInfo.ranking == 1)
            {
                textObjWhenWin.SetActive(true);
            }
            else
            {
                textObjWhenLose.SetActive(true);
            }
        }
    }
}

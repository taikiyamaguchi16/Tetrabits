using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class RaceFinish : MonoBehaviourPunCallbacks
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
    
    public bool goaled { get; private set; } = false;

    [SerializeField]
    float standBytimeSeconds = 3f;

    float standByTimer = 0f;

    bool winFlag = false;

    [SerializeField]
    bool finalStage = false;

    bool sceneShifted = false;

    // Update is called once per frame
    void Update()
    {
        if (goaled)
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
                        if (PhotonNetwork.IsMasterClient && !sceneShifted)
                        {
                            sceneShifted = true;
                            GameInGameUtil.SwitchGameInGameScene(nextStageScene);
                        }
                    }
                }
                else
                {
                    if(nowStageScene != null)
                    {
                        if (PhotonNetwork.IsMasterClient && !sceneShifted)
                        {
                            sceneShifted = true;
                            GameInGameUtil.SwitchGameInGameScene(nowStageScene);
                            MonitorManager.DealDamageToMonitor("large");
                        }
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        // ゴール時のみ
        if (playerInfo.lapCounter.goaled && !goaled)   
        {
            photonView.RPC(nameof(RPCWhenGoal), RpcTarget.AllViaServer);
        }
    }

    [PunRPC]
    private void RPCWhenGoal()
    {
        raceManager.RankingCalculation();

        goaled = true;
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

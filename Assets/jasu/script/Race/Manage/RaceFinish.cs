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
                        GameInGameUtil.StopGameInGameTimer("race");
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
            goaled = true;
            if (PhotonNetwork.IsMasterClient)
            {
                raceManager.RankingCalculation();
                photonView.RPC(nameof(RPCWhenGoal), RpcTarget.All, playerInfo.ranking);
            }
        }
    }

    [PunRPC]
    private void RPCWhenGoal(int _ranking)
    {
        playerInfo.ranking = _ranking;

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

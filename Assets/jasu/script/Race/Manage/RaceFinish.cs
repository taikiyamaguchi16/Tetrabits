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
    RacerInfo[] racerInfos;

    [SerializeField]
    GameObject textObjWhenWin;

    [SerializeField]
    GameObject textObjWhenLose;

    [SerializeField]
    GameObject textObjWhenLoseOthersGoal;

    public bool goaled { get; private set; } = false;

    [SerializeField]
    float standBytimeSeconds = 3f;

    float standByTimer = 0f;

    bool winFlag = false;

    [SerializeField]
    bool finalStage = false;

    bool sceneShifted = false;

    [SerializeField]
    AudioClip seWin;

    [SerializeField]
    AudioClip seLose;

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
                        //if (PhotonNetwork.IsMasterClient)
                        //{
                        //    photonView.RPC(nameof(RPCStopGameTimer), RpcTarget.All);
                        //}
                        GameInGameUtil.StopGameInGameTimer("race");
                        GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                    }
                    else if (nextStageScene != null)
                    {
                        if (PhotonNetwork.IsMasterClient && !sceneShifted)
                        {
                            sceneShifted = true;
                            GameInGameUtil.SwitchGameInGameScene(nextStageScene);   //シーン遷移
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
                            MonitorManager.CallAddNumDebrisInGameMainStage();   // 破片の落下数を増やす
                            //MonitorManager.DealDamageToMonitor("large");
                        }
                    }
                }
            }
        }
    }

    private void LateUpdate()
    {
        foreach(RacerInfo racerInfo in racerInfos)
        {
            // ゴール時のみ
            if (racerInfo.lapCounter.goaled && !goaled)
            {
                goaled = true;
                if (PhotonNetwork.IsMasterClient)
                {
                    raceManager.RankingCalculation();
                    photonView.RPC(nameof(RPCWhenGoal), RpcTarget.All, playerInfo.ranking);
                }

                break;
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
            SimpleAudioManager.PlayOneShot(seWin);
        }
        else
        {
            textObjWhenLose.SetActive(true);
            textObjWhenLoseOthersGoal.SetActive(true);
            SimpleAudioManager.PlayOneShot(seLose);
        }
    }

    //[PunRPC]
    //private void RPCStopGameTimer()
    //{
    //    GameInGameUtil.StopGameInGameTimer("race");
    //}
}

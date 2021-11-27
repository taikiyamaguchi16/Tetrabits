using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LapCounter : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform checkPoint = null;

    [SerializeField, Tooltip("通過判定の計算をしだすチェックポイントとの距離")]
    float passJudgeRange = 100f;

    [SerializeField]
    RaceManager raceManager;

    [SerializeField]
    BikeCtrlWhenStartAndGoal bikeCtrlWhenStartAndGoal;

    //public int actualLapCount { get; private set; } = 0;

    //public int lapCount { get; private set; } = 0;

    public int actualLapCount = 0;

    public int lapCount = 0;

    Vector3 dirOfLane;  // 進行方向

    Vector3 oldPos;

    public bool goaled { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        dirOfLane = checkPoint.forward;
        oldPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        LapCount();
    }
    
    public void LapCount()
    {
        if (Vector3.Distance(checkPoint.position, transform.position) < passJudgeRange &&
            Vector3.Distance(transform.position, oldPos) < passJudgeRange) // 範囲を超える瞬間移動は通過とみなさない
        {
            // 順方向に通過したとき
            if (Vector3.Dot(transform.position - checkPoint.position, dirOfLane) > 0 &&
                Vector3.Dot(oldPos - checkPoint.position, dirOfLane) <= 0)
            {
                // actualLapCountと同じならラップを加算する
                if (actualLapCount == lapCount)
                {
                    if (PhotonNetwork.IsMasterClient)
                    {
                        lapCount++;
                        photonView.RPC(nameof(RPCLapCount), RpcTarget.AllViaServer, lapCount);
                    }

                    if (PhotonNetwork.IsMasterClient && lapCount > raceManager.GetLapNum)    // ゴール判定
                    {
                        goaled = true;
                        photonView.RPC(nameof(RPCGoal), RpcTarget.AllViaServer, goaled);
                    }
                }

                if (PhotonNetwork.IsMasterClient)
                {
                    actualLapCount++;
                    photonView.RPC(nameof(RPCActualLapCount), RpcTarget.AllBufferedViaServer, actualLapCount);
                }
                //actualLapCount++;
            }
            else if (Vector3.Dot(transform.position - checkPoint.position, dirOfLane) <= 0 &&
                Vector3.Dot(oldPos - checkPoint.position, dirOfLane) > 0) // 逆走したらactualLapCountをマイナス
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    actualLapCount--;
                    photonView.RPC(nameof(RPCActualLapCount), RpcTarget.AllViaServer, actualLapCount);
                }
                //actualLapCount--;
            }
        }

        oldPos = transform.position;
    }

    [PunRPC]
    private void RPCLapCount(int _lapCount)
    {
        lapCount = _lapCount;
    }
    
    [PunRPC]
    private void RPCActualLapCount(int _actualLapCount)
    {
        actualLapCount = _actualLapCount;
    }

    [PunRPC]
    private void RPCGoal(bool _goalFlag)
    {
        goaled = _goalFlag;
        if(goaled)
        {
            bikeCtrlWhenStartAndGoal.SetActiveOffAfterGoal(false);
            bikeCtrlWhenStartAndGoal.SetActiveOnAfterGoal(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaceManager : MonoBehaviourPunCallbacks
{
    struct Progress
    {
        public float progressVal;
        public int index;
    }

    [SerializeField]
    GameObject[] racers;

    public GameObject[] GetRacers { get { return racers; } }

    RacerInfo[] racersInfo;

    [SerializeField]
    RaceFinish raceFinish;

    [SerializeField]
    RaceStageMolder raceStageMolder;

    [SerializeField]
    Transform startPoint;
    
    [Header("設定項目")]

    [SerializeField, Tooltip("レースのラップ数")]
    int lapNum = 3;

    public int GetLapNum { get { return lapNum; } }

    // Start is called before the first frame update
    void Start()
    {
        racersInfo = new RacerInfo[racers.Length];
        for(int i = 0; i < racers.Length; i++)
        {
            racersInfo[i] = racers[i].GetComponent<RacerInfo>();
        }
    }

    private void LateUpdate()
    {
        if (!raceFinish.goaled)
        {
            // 順位計算
            RankingCalculation();
        }
    }

    //public void RankingCalculationForRPCOther()
    //{
    //    photonView.RPC(nameof(RPCRankingCalculation), RpcTarget.All);
    //}

    //[PunRPC]
    //public void RPCRankingCalculation()
    //{
    //    RankingCalculation();
    //}

    public void RankingCalculation()
    {
        Progress[] racersProgress = new Progress[racers.Length];

        for (int i = 0; i < racers.Length; i++)
        {
            racersProgress[i].index = i;

            float posZInLane = racers[i].transform.position.z - startPoint.position.z;
            if (posZInLane < 0)
            {
                posZInLane += raceStageMolder.GetLaneLength;
            }
            racersProgress[i].progressVal = racersInfo[i].lapCounter.actualLapCount * raceStageMolder.GetLaneLength + posZInLane;
        }

        // 降順ソート
        for (int i = 0; i < racers.Length; i++)
        {
            for (int j = i + 1; j < racers.Length; j++)
            {
                if (racersProgress[i].progressVal < racersProgress[j].progressVal)
                {
                    Progress tmp = racersProgress[i];
                    racersProgress[i] = racersProgress[j];
                    racersProgress[j] = tmp;
                }
            }
        }

        for (int i = 0; i < racers.Length; i++)
        {
            racersInfo[racersProgress[i].index].ranking = i + 1;
        }
    }
}

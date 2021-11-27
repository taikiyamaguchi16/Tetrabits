using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineManager : MonoBehaviour
{
    [SerializeField]
    Transform startLineTrans;

    [SerializeField]
    RaceManager raceManager = null;

    [SerializeField]
    LaneManager laneManager = null;
    
    GameObject[] racers;

    // Start is called before the first frame update
    void Start()
    {
        racers = raceManager.GetRacers;

        // レーサー整列
        for(int i = 0; i < racers.Length; i++)
        {
            if(i < laneManager.GetLaneNum())
            {
                Vector3 pos = racers[i].transform.localPosition;
                pos.x = laneManager.GetLaneLocalPosX(i);
                pos.y = 0f;
                pos.z = startLineTrans.localPosition.z - laneManager.GetLaneWidth() / 2 * (i + 2.5f);
                racers[i].transform.localPosition = pos;

                racers[i].GetComponentInChildren<MoveBetweenLane>().belongingLaneId = i;
            }
        }
    }
}

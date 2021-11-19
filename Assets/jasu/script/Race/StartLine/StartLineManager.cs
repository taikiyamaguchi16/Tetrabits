﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLineManager : MonoBehaviour
{
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
                pos.z = transform.localPosition.z - laneManager.GetLaneWidth() / 2 * (i + 1);
                racers[i].transform.localPosition = pos;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

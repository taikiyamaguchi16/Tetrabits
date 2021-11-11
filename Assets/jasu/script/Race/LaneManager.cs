﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] lanes;

    public float GetLanePosX(int _laneId)
    {
        return lanes[_laneId].transform.position.x;
    }

    public int GetLaneNum()
    {
        return lanes.Length;
    }
}
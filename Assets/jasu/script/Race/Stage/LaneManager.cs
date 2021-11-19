using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaneManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] lanes;

    [SerializeField]
    RaceStageMolder raceStageMolder = null;

    private void Awake()
    {
        if (raceStageMolder != null)
        {
            lanes = raceStageMolder.GetLanes;
        }
    }

    public float GetLanePosX(int _laneIndex)
    {
        return lanes[_laneIndex].transform.position.x;
    }

    public float GetLaneLocalPosX(int _laneIndex)
    {
        return lanes[_laneIndex].transform.localPosition.x;
    }

    public int GetLaneNum()
    {
        return lanes.Length;
    }

    public float GetLaneWidth()
    {
        if(raceStageMolder != null)
        {
            return raceStageMolder.GetRaceObjWidth;
        }
        return 1f;
    }
}

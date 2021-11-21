using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplashSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject dirtSplashPrefab = null;

    [SerializeField]
    RaceStageMolder raceStageMolder;

    [SerializeField]
    GameObject dirtParentObj = null;    // 泥が着地時に生成される泥だまりの親obj stage

    [SerializeField]
    GameObject player = null;
    

    public void InstantiateDirtSplash(Vector3 _moveForce)
    {
        GameObject dirtSplashObj = Instantiate(dirtSplashPrefab);
        dirtSplashObj.transform.position = transform.position;
        DirtSplash dirtSplash = dirtSplashObj.GetComponent<DirtSplash>();
        dirtSplash.parentInstanceID = gameObject.GetInstanceID();
        dirtSplash.parentMoveForce = _moveForce;
        dirtSplash.raceStageMolder = raceStageMolder;
        dirtSplash.parentObj = dirtParentObj;
        dirtSplash.laneLength = raceStageMolder.GetLaneLength;
        PositionCorrectionWhenWarpPlayer posCorrect = dirtSplashObj.GetComponent<PositionCorrectionWhenWarpPlayer>();
        posCorrect.player = player;
        posCorrect.raceStageMolder = raceStageMolder;
    }
}

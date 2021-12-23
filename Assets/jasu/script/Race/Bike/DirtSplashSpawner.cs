using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DirtSplashSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject dirtSplashPrefab = null;

    [SerializeField]
    RaceStageMolder raceStageMolder;

    //[SerializeField]
    //GameObject dirtParentObj = null;    // 泥が着地時に生成される泥だまりの親obj stage

    [SerializeField]
    GameObject[] dirtSplashParents;

    [SerializeField]
    GameObject player = null;

    [SerializeField]
    float dirtSplashForceMultiply = 0.5f;

    public void InstantiateDirtSplash(Vector3 _moveForce)
    {
        //GameObject dirtSplashObj = Instantiate(dirtSplashPrefab);
        //GameObject dirtSplashObj = Instantiate(dirtSplashPrefab);
        //GameInGameUtil.MoveGameObjectToOwnerScene(dirtSplashObj, gameObject); ;
        //dirtSplashObj.transform.position = transform.position;
        //DirtSplash dirtSplash = dirtSplashObj.GetComponent<DirtSplash>();
        //dirtSplash.parentInstanceID = gameObject.GetInstanceID();
        //dirtSplash.parentMoveForce = _moveForce * dirtSplashForceMultiply;
        //dirtSplash.raceStageMolder = raceStageMolder;
        ////dirtSplash.parentObj = dirtParentObj;
        //dirtSplash.parentObjs = dirtSplashParents;
        //dirtSplash.laneLength = raceStageMolder.GetLaneLength;
        //PositionCorrectionWhenWarpPlayer posCorrect = dirtSplashObj.GetComponent<PositionCorrectionWhenWarpPlayer>();
        //posCorrect.player = player;
        //posCorrect.raceStageMolder = raceStageMolder;
    }
}

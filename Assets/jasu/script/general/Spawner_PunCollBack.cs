using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Spawner_PunCollBack : MonoBehaviourPunCallbacks
{
    [SerializeField]
    List<Transform> playerSpaownList = new List<Transform>();

    [SerializeField]
    GameObject spoawnObj;

    public override void OnJoinedRoom()
    {
        Transform _pos = playerSpaownList[PhotonNetwork.PlayerList.Length-1];
        GameObject spawned = PhotonNetwork.Instantiate(spoawnObj.name, _pos.position, Quaternion.Euler(_pos.eulerAngles));
    }
 
}

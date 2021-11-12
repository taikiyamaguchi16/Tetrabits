using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomCreateWait : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Header("カーソル入れる")]
    GameObject cursor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnJoinedLobby()
    {
        cursor.SetActive(true);
    }
}

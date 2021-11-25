using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class RoomCreateWait : MonoBehaviourPunCallbacks
{
    [SerializeField]
    [Header("ルーム入室などの際消しておきたいオブジェクトを入れる")]
    GameObject[] DeactiveObjects;

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
        for(int i = 0; i < DeactiveObjects.Length; i++)
        {
            DeactiveObjects[i].SetActive(true);
        }
    }

    public void OnJoiningRoom()
    {
        for (int i = 0; i < DeactiveObjects.Length; i++)
        {
            DeactiveObjects[i].SetActive(false);
        }
    }

    public override void OnJoinedRoom()
    {
        for (int i = 0; i < DeactiveObjects.Length; i++)
        {
            DeactiveObjects[i].SetActive(true);
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        for (int i = 0; i < DeactiveObjects.Length; i++)
        {
            DeactiveObjects[i].SetActive(true);
        }
    }
}

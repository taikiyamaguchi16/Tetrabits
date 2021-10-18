using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class CassetteManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    //ルームにプレイヤーが参加したとき
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(PhotonNetwork.PlayerList.Length + newPlayer.NickName);
    }
}
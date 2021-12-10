﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class WaitOnlinePlayer : MonoBehaviour
{
    private const int _PLAYER_UPPER_LIMIT = 4;

    [SerializeField]
    GameObject Own;

    [SerializeField]
    Text nameText;

    Player[] players;

    // Start is called before the first frame update
    void Start()
    {
        players = PhotonNetwork.PlayerList;
    }

    // Update is called once per frame
    void Update()
    {
        if (Own.name == "Keirei_Yellow(Clone)")
        {
            nameText.text = players[0].NickName;
            nameText.color = Color.yellow;
        }

        if (Own.name == "Keirei_Red(Clone)")
        {
            nameText.text = players[1].NickName;
            nameText.color = Color.red;
        }

        if (Own.name == "Keirei_Green(Clone)")
        {
            nameText.text = players[2].NickName;
            nameText.color = Color.green;
        }

        if (Own.name == "Keirei_Blue(Clone)")
        {
            nameText.text = players[3].NickName;
            nameText.color = Color.blue;
        }

        //for (int i = 0; i < players.Length; i++)
        //{
        //    Debug.Log("プレイヤー" + i + "：" + players[i].NickName);



        //    switch (i)
        //    {
        //        case 0:

        //            break;

        //        case 1:

        //            break;

        //        case 2:

        //            break;

        //        case 3:

        //            break;
        //    }
        //}
    }
}

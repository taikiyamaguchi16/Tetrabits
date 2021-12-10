using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class DisplayPlayerNum : MonoBehaviour
{
    [SerializeField]
    Text playerNumText;

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.InRoom)
            playerNumText.text = "参加人数" + "\n" + PhotonNetwork.CurrentRoom.PlayerCount + " / 4";
    }
}

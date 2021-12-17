using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditGameMainPlayerNameSetter : MonoBehaviour
{
    [SerializeField] int playerIndex;
    [SerializeField] Text creditPlayerName;

    private void Awake()
    {
        foreach (var player in Photon.Pun.PhotonNetwork.PlayerList)
        {
            if (playerIndex == player.GetPlayerNum())
            {
                creditPlayerName.text = player.NickName;
                break;
            }
        }
    }
}

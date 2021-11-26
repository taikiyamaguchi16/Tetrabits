using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpNeedleBlock : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージ量")] string damage = "small";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            MonitorManager.DealDamageToMonitor(damage);
        }
    }
}
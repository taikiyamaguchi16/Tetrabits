using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerClashSensor : MonoBehaviourPunCallbacks
{
    [SerializeField]
    RacerController racerController;

    [SerializeField]
    Vector3 impalseForce;

    Rigidbody racerRb;

    public bool playerClash { get; private set; } = false;

    [SerializeField]
    float clashStopSeconds = 2f;

    float clashStopTimer;

    [SerializeField]
    float playerSlipSeconds = 2.1f;

    [SerializeField]
    float rotNum = 3f;

    private void Start()
    {
        racerRb = racerController.GetRigidbody();
    }

    private void Update()
    {
        if (playerClash)
        {
            clashStopTimer += Time.deltaTime;
            if (clashStopTimer > clashStopSeconds)
            {
                playerClash = false;
                clashStopTimer = 0f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            if (!playerClash)
            {
                racerController.GetBikeSlipDown().CallSlipStart("medium", playerSlipSeconds, 360f * rotNum);

                if (PhotonNetwork.IsMasterClient)
                {
                    //MonitorManager.CallAddNumDebrisInGameMainStage();
                    photonView.RPC(nameof(RPCClash), RpcTarget.All);
                }
            }
        }
    }

    [PunRPC]
    private void RPCClash()
    {
        playerClash = true;
        racerRb.AddForce(impalseForce, ForceMode.Impulse);
    }
}

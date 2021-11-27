using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DirtSplashSpawnInInput : DirtSplashSpawn
{
    // Update is called once per frame
    void Update()
    {
        if(colliderSensorFront.GetExistInCollider() ||
            colliderSensorFront.GetExistInCollider())
        {
            if (TetraInput.sTetraButton.GetTrigger())
            {
                photonView.RPC(nameof(RPCInstantiateDirtSplash), RpcTarget.All);
            }
        }
    }
}

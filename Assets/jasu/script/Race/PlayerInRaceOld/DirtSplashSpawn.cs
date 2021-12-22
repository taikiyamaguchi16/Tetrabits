using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DirtSplashSpawn : MonoBehaviourPunCallbacks
{
    [SerializeField]
    protected DirtSplashSpawner dirtSplashSpawner = null;

    [SerializeField]
    protected MoveInRace moveInRace;

    public Vector3 moveVec { get; set; }

    [SerializeField]
    protected float downSpdMultiply = 0.5f;

    public bool dirtSplashFlag { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (dirtSplashFlag)
        {
            dirtSplashFlag = false;
            photonView.RPC(nameof(RPCInstantiateDirtSplash), RpcTarget.All);
        }
    }

    [PunRPC]
    protected void RPCInstantiateDirtSplash()
    {
        dirtSplashSpawner.InstantiateDirtSplash(moveVec);
        Vector3 velocity = moveInRace.rb.velocity;
        velocity.z *= downSpdMultiply;
        moveInRace.rb.velocity = velocity;
    }
}

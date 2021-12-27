using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class RaceEnemyDirtAttack : MonoBehaviourPunCallbacks
{
    bool isAttacking = false;

    [SerializeField]
    SpriteBlink spriteBlink;

    [SerializeField]
    GameObject dirtSplashPrefab;

    [SerializeField]
    float warningTimeSeconds = 3f;

    float warningTimer = 0f;

    [SerializeField]
    float dirtInterval = 0.1f;

    float dirtTimer = 0f;

    [SerializeField]
    int dirtNum = 25;

    int dirtCounter = 0;

    [SerializeField]
    float dirtPosY = 70f; 

    // Start is called before the first frame update
    void Start()
    {
        spriteBlink.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isAttacking && PhotonNetwork.IsMasterClient)
        {
            warningTimer += Time.deltaTime;
            if(warningTimer > warningTimeSeconds)
            {
                spriteBlink.active = false;

                dirtTimer += Time.deltaTime;
                if(dirtTimer > dirtInterval)
                {
                    dirtTimer = 0f;

                    Vector3 pos = transform.position;
                    pos.y = dirtPosY;
                    pos.z -= 3f * dirtCounter;

                    photonView.RPC(nameof(RPCGenelateDirt), RpcTarget.All, pos);

                    dirtCounter++;
                    if(dirtCounter >= dirtNum)
                    {
                        warningTimer = 0f;
                        dirtTimer = 0f;
                        dirtCounter = 0;
                        isAttacking = false;
                    }
                }
            }
        }
    }

    public void AttackStart()
    {
        if (!isAttacking && PhotonNetwork.IsMasterClient)
        {
            photonView.RPC(nameof(RPCDirtAttackStart), RpcTarget.All);
        }
    }

    [PunRPC]
    public void RPCDirtAttackStart()
    {
        warningTimer = 0f;
        dirtTimer = 0f;
        dirtCounter = 0;
        isAttacking = true;
        spriteBlink.active = true;
    }

    [PunRPC]
    public void RPCGenelateDirt(Vector3 _pos)
    {
        GameObject dirtSplashObj = Instantiate(dirtSplashPrefab);
        GameInGameUtil.MoveGameObjectToOwnerScene(dirtSplashObj, gameObject);
        dirtSplashObj.transform.parent = transform;

        dirtSplashObj.transform.position = _pos;
    }
}

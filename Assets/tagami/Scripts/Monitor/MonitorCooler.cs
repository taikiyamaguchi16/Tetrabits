using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MonitorCooler : MonoBehaviourPunCallbacks, IPlayerAction
{
    [Header("Required Reference")]
    [SerializeField] MonitorManager monitorManager;
    [SerializeField] ParticleSystem coolingEffect;
    [SerializeField] Transform rotateTarget;

    [Header("Status")]
    [SerializeField] float damageToCoolingTargetPerSeconds = 1.0f;
    [SerializeField] float repairMonitorPerSeconds = 0.1f;
    [SerializeField] float rotateAnglePerSeconds = 10.0f;

    //int controlXinputIndex = 0;
    bool running = false;
    CoolerRotater runningRotator;

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (!coolingEffect.isPlaying)
            {
                coolingEffect.Play();
            }

            //レイとばして冷却ターゲット削除
            Debug.DrawRay(rotateTarget.position, rotateTarget.forward, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(new Ray(rotateTarget.position, rotateTarget.forward), out hit, 1000.0f))
            {
                //Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.CompareTag("CoolingTarget"))
                {
                    if (hit.collider.gameObject.GetComponent<CoolingTargetStatus>().TryToKill(damageToCoolingTargetPerSeconds * Time.deltaTime))
                    {
                        if (PhotonNetwork.IsMasterClient)
                        {
                            monitorManager.CallRepairMonitor(hit.collider.gameObject.GetComponent<CoolingTargetStatus>().damageToMonitor);
                        }
                        Destroy(hit.collider.gameObject);
                    }
                }
            }

        }
        else
        {
            if (coolingEffect.isPlaying)
            {
                coolingEffect.Stop();
            }
        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        //おそらくIsMineで呼ばれてるので同期関数をそのまま呼び出す

        Debug.Log("CoolingDeviceのStartPlayerActionが呼ばれました");
        CallSetRunning(true);

        runningRotator = _desc.playerObj.AddComponent<CoolerRotater>();
        runningRotator.rotateTarget = rotateTarget;

        //var playerMove = _desc.playerObj.GetComponent<PlayerMove>();
        //if (playerMove)
        //{
        //    controlXinputIndex = playerMove.controllerID;
        //}
        //else
        //{
        //    Debug.LogError("PlayerにPlayerMoveのスクリプトがアタッチされてない");
        //}
    }

    public void EndPlayerAction(PlayerActionDesc _desc)
    {
        CallSetRunning(false);

        Destroy(runningRotator);
    }

    public int GetPriority()
    {
        return 50;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        return true;
    }

    void CallSetRunning(bool _value)
    {
        photonView.RPC(nameof(RPCSetRunning), RpcTarget.AllViaServer, _value);
    }
    [PunRPC]
    void RPCSetRunning(bool _value)
    {
        running = _value;
    }

}

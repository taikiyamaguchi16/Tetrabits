using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.VFX;

public class BatterySpowner : MonoBehaviourPunCallbacks, IPlayerAction
{
    //生成するオブジェクト
    [SerializeField]
    GameObject spownObj;
    //保有しているバッテリー
    private Battery ownBattery;
    //保有用のポケット
    ItemPocket pocket;
    //バッテリーの生成時間
    [SerializeField]
    float spownBatteryTime;
    //経過時間
    private float elpsedTime;

    //エフェクト再生からの経過時間
    private float elpsedEfectTime;
    //エフェクト再生から何秒でとれるか
    [SerializeField]
    float canBatteryActionTime;

    private bool isPlayEfect;

    [SerializeField]
    BatteryLift batteryLift;
    [SerializeField]
    VisualEffect smokeEfect;
    //生成可能かどうか
    private bool canSpawn;
    // Start is called before the first frame update

    public void Start()
    {
        canSpawn = false;
    }
    public  void StartSpawn()
    {
        pocket = GetComponent<ItemPocket>();
        //エフェクトの再生
        smokeEfect.Play();
        if (PhotonNetwork.IsMasterClient)
        {
            SpawonBattery();
            canSpawn = true;
        }
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (ownBattery == null && canSpawn)
            {
                elpsedTime += Time.deltaTime;
                //リフトの位置決定
                batteryLift.SetBatteryLiftPos(elpsedTime, spownBatteryTime);
                if (spownBatteryTime < elpsedTime)
                {
                    photonView.RPC(nameof(RPCPlaySmokeEfect), RpcTarget.All);

                    elpsedTime = 0f;
                    SpawonBattery();
                }          
            }
        }
        elpsedEfectTime += Time.deltaTime;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        //エフェクト再生中には取れないように
        if (elpsedEfectTime>canBatteryActionTime)
        {
            photonView.RPC(nameof(RPCSpownerBatteryAction), RpcTarget.AllBufferedViaServer, _desc.playerObj.GetPhotonView().ViewID);   
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 110;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        //エフェクト再生中には取れないように
        if (elpsedEfectTime > canBatteryActionTime)
        {
            ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
            //プレイヤーが何も持っていない場合
            if (otherPocket.GetItem() == null)
            {
                //バッテリーが生成されていた場合
                if (ownBattery != null)
                {
                    return true;
                }                
            }
        }
        return false;
    }

    //バッテリーの残量を返す
    public float GetBatterylevel()
    {
        if (ownBattery != null)
            return ownBattery.GetLevel();
        else
            return 0f;
    }

    private void SpawonBattery()
    {
        var b_obj = PhotonNetwork.InstantiateRoomObject(spownObj.name, new Vector3(100,100,100), Quaternion.identity);
        PhotonNetwork.AllocateViewID(b_obj.GetPhotonView().ViewID);

        photonView.RPC(nameof(RPCSpawonBattery), RpcTarget.All, b_obj.GetPhotonView().ViewID);
        
        elpsedTime = 0f;
    }

    [PunRPC]
    public void RPCSpawonBattery(int _id)
    {
        ownBattery = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<Battery>();
        ownBattery.CallPickUp(photonView.ViewID);      
    }

    [PunRPC]
    public void RPCPlaySmokeEfect()
    {
        //エフェクトの再生
        smokeEfect.SendEvent("OnPlay");
        isPlayEfect = true;
        elpsedEfectTime = 0f;
    }

    [PunRPC]
    public void RPCStolenOwnBattery()
    {
        //保持しているバッテリーを放棄
        pocket.SetItem(null);
        ownBattery = null;
    }

    [PunRPC]
    public void RPCSpownerBatteryAction(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ItemPocket otherPocket = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<ItemPocket>();
            //プレイヤーが何も持っていない場合
            if (otherPocket.GetItem() == null)
            {
                //バッテリーが生成されていた場合
                if (ownBattery != null)
                {
                    ownBattery.CallPickUp(_id);
                    photonView.RPC(nameof(RPCStolenOwnBattery), RpcTarget.All);

                    //エフェクトの経過時間をリセット                    
                    isPlayEfect = false;
                }
            }

        }
    }
}

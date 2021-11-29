using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatteryHolder : MonoBehaviourPunCallbacks, IPlayerAction
{
    private Battery ownBattery;

    ItemPocket pocket;

    private  bool checkSetBatteryMaster;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
        checkSetBatteryMaster = false;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        //ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        ////プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
        //Battery checkbattery = ownBattery;
        ////プレイヤーが何か持っていた場合
        //if (otherPocket.GetItem() != null)
        //{
        //    ownBattery = otherPocket.GetItem().GetComponent<Battery>();
        //    //渡されたのがバッテリーだった場合
        //    if (ownBattery != null)
        //    {
        //        ownBattery.CallDump(_desc.playerObj.GetPhotonView().ViewID);
        //        ownBattery.CallPickUp(photonView.ViewID);
        //        otherPocket.SetItem(null);
        //        //自分がバッテリを持っていた場合swapする
        //        if (checkbattery != null)
        //        {
        //            checkbattery.CallPickUp(_desc.playerObj.GetPhotonView().ViewID);
        //        }

        //        //他のプレイヤーのホルダーにもバッテリーをセット
        //        photonView.RPC(nameof(RPCSetOwnBattery), RpcTarget.Others, ownBattery.photonView.ViewID);
        //    }
        //    //バッテリーでなかった場合元に戻す
        //    else
        //    {
        //        ownBattery = checkbattery;
        //    }
        //}
        ////何も持っていなかった場合自分のを渡す
        //else
        //{
        //    //自分がバッテリを持っていた場合渡す
        //    if (ownBattery != null)
        //    {
        //        ownBattery.CallPickUp(_desc.playerObj.GetPhotonView().ViewID);

        //        pocket.SetItem(null);
        //        ownBattery = null;

        //        //他のプレイヤーのホルダーのバッテリーを抜く
        //        photonView.RPC(nameof(RPCReleaseBattery), RpcTarget.Others);
        //    }
        //}
        photonView.RPC(nameof(RPCBatteryAction), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 110;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
        Battery checkbattery = ownBattery;
        Battery possibleBattery;
        //プレイヤーが何か持っていた場合
        if (otherPocket.GetItem() != null)
        {
            possibleBattery = otherPocket.GetItem().GetComponent<Battery>();
            //渡されたのがバッテリーだった場合
            if (possibleBattery != null)
            {
                return true;
            }
            //バッテリーでなかった場合元に戻す
            else
            {            
                return false;
            }
        }
        //何も持っていなかった場合自分のを渡す
        else
        {
            //自分がバッテリを持っていた場合渡す
            if (ownBattery != null)
            {
                return true;
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

    [PunRPC]
    private void RPCSetOwnBattery(int _id)
    {
        ownBattery = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<Battery>();
    }

    [PunRPC]
    private void RPCReleaseBattery()
    {
        pocket.SetItem(null);
        ownBattery = null;
    }

    public void ConsumptionOwnBattery(float _consumption)
    {
        if (ownBattery != null)
            ownBattery.BatteryConsumption(_consumption);
    }

    [PunRPC]
    private void RPCBatteryAction(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            ItemPocket otherPocket = NetworkObjContainer.NetworkObjDictionary[_id].GetComponent<ItemPocket>();
            //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
            Battery checkbattery = ownBattery;
            //プレイヤーが何か持っていた場合
            if (otherPocket.GetItem() != null)
            {
                ownBattery = otherPocket.GetItem().GetComponent<Battery>();
                //渡されたのがバッテリーだった場合
                if (ownBattery != null)
                {
                    ownBattery.CallDump(_id);
                    ownBattery.CallPickUp(photonView.ViewID);
                    otherPocket.SetItem(null);
                    //自分がバッテリを持っていた場合swapする
                    if (checkbattery != null)
                    {
                        checkbattery.CallPickUp(_id);
                    }

                    //他のプレイヤーのホルダーにもバッテリーをセット
                    photonView.RPC(nameof(RPCSetOwnBattery), RpcTarget.Others, ownBattery.photonView.ViewID);
                }
                //バッテリーでなかった場合元に戻す
                else
                {
                    ownBattery = checkbattery;
                }
            }
            //何も持っていなかった場合自分のを渡す
            else
            {
                //自分がバッテリを持っていた場合渡す
                if (ownBattery != null)
                {
                    ownBattery.CallPickUp(_id);

                    pocket.SetItem(null);
                    ownBattery = null;

                    //他のプレイヤーのホルダーのバッテリーを抜く
                    photonView.RPC(nameof(RPCReleaseBattery), RpcTarget.Others);
                }
            }
        }
    }
}

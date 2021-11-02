using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatteryDustBox : MonoBehaviour, IPlayerAction
{
    private Battery ownBattery;

    ItemPocket pocket;
    // Start is called before the first frame update
    void Start()
    {
        pocket = GetComponent<ItemPocket>();
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        //プレイヤーに自身が持ってたオブジェクトを渡すための一時保存用
        Battery checkbattery = ownBattery;
        //プレイヤーが何か持っていた場合
        if (otherPocket.GetItem() != null)
        {
            ownBattery = otherPocket.GetItem().GetComponent<Battery>();
            //渡されたのがバッテリーだった場合
            if (ownBattery != null)
            {
                ownBattery.PickUp(this.gameObject);
                otherPocket.SetItem(null);

                PhotonNetwork.Destroy(ownBattery.photonView);
                ownBattery = null;
            }
            //バッテリーでなかった場合元に戻す
            else
                ownBattery = checkbattery;
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 120;
    }

    //バッテリーの残量を返す
    public float GetBatterylevel()
    {
        if (ownBattery != null)
            return ownBattery.GetLevel();
        else
            return 0f;
    }
}

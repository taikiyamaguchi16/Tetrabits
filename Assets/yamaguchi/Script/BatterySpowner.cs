using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class BatterySpowner : MonoBehaviourPunCallbacks, IPlayerAction
{

    [SerializeField]
    GameObject spownObj;
    private Battery ownBattery;
  
    ItemPocket pocket;

    [SerializeField]
    float spownBatteryTime;

    private float elpsedTime;
    // Start is called before the first frame update
    public override void OnJoinedRoom()
    {

        pocket = GetComponent<ItemPocket>();

        //var b_obj = GameObject.Instantiate(spownObj);
        var b_obj = PhotonNetwork.Instantiate(spownObj.name, Vector3.zero, Quaternion.identity);

        pocket.SetItem(b_obj);
        ownBattery = b_obj.GetComponent<Battery>();

        elpsedTime = 0f;

    }

    private void Update()
    {
        if (ownBattery == null)
        {
            elpsedTime += Time.deltaTime;
            if (spownBatteryTime < elpsedTime)
            {
                //var b_obj = GameObject.Instantiate(spownObj);
                var b_obj = PhotonNetwork.Instantiate(spownObj.name, Vector3.zero, Quaternion.identity);
                pocket.SetItem(b_obj);
                ownBattery = b_obj.GetComponent<Battery>();

                elpsedTime = 0f;
            }
        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        ItemPocket otherPocket = _desc.playerObj.GetComponent<ItemPocket>();
        //プレイヤーが何も持っていない場合
        if (otherPocket.GetItem() == null)
        {     
            //バッテリーが生成されていた場合
            if (ownBattery != null)
            {
                ownBattery.PickUp(_desc.playerObj);
                pocket.SetItem(null);
                ownBattery = null;
            }
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return 100;
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

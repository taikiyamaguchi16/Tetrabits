using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

[System.Serializable]
public struct BatteryLevelfromColor
{
    public float batteryLevel;
    [ColorUsage(false, true)]  public Color batteryColor;
}

public class Battery : MonoBehaviourPunCallbacks, IPlayerAction
{
    [SerializeField]
    private GameObject energyGazeObj;
    private float energyGazeSize;
    private float energyGazePos;

    private float elpsedTime;

    private Rigidbody rb;
    private Collider col;
    [SerializeField, ReadOnly]
    //保有されているか
    public bool isOwned;

    [SerializeField]
    private int priority;

    //電池残量
    [SerializeField, ReadOnly]
    private float level = 100f;

    [SerializeField]
    private Text actionText;
    [SerializeField]
    ControlUIActivator myControlUi;

    [SerializeField]
    float throwForce;
    [SerializeField]
    float throwUpForce;

    [SerializeField]
    AudioClip throwBatterrySe;
    private Vector3 playerDir;

    private ItemPocket ownerSc;

    [SerializeField]
    List<BatteryLevelfromColor> batteryLevelfromColors = new List<BatteryLevelfromColor>();

    [SerializeField]
    MeshRenderer batteryMaterial;
    private void Update()
    {
        if (!isOwned)
        {
            actionText.text = "拾う";
        }
        else
        {
            actionText.text = "投げる";
        }

        if (PhotonNetwork.IsMasterClient)
        {
            elpsedTime += Time.deltaTime;
            //1秒に一回くらい同期取る
            if (elpsedTime > 1f)
            {
                photonView.RPC(nameof(RPCSetBatteryLevel), RpcTarget.Others, level);
                elpsedTime = 0f;
            }

        }

        //残量に応じた色の変更
        if (level < batteryLevelfromColors[0].batteryLevel)
        {
            batteryMaterial.material.SetColor("_EmissionColor", batteryLevelfromColors[0].batteryColor);       
            if(level<batteryLevelfromColors[1].batteryLevel)
            {
                batteryMaterial.material.SetColor("_EmissionColor", batteryLevelfromColors[1].batteryColor);
            }
        }
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;
        level = 100f;
        elpsedTime = 0f;

        energyGazeSize = energyGazeObj.transform.localScale.y;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {      
        photonView.RPC(nameof(RPCBatteryPlayAction), RpcTarget.All, _desc.playerObj.GetPhotonView().ViewID);
    }
    [PunRPC]
    private void RPCBatteryPlayAction(int _id)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (!isOwned)
            {
                photonView.RPC(nameof(PickUp), RpcTarget.AllBufferedViaServer, _id);
            }
            else
            {
                photonView.RPC(nameof(Dump), RpcTarget.AllBufferedViaServer, _id);
            }
        }
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return priority;
    }

    public bool GetIsActionPossible(PlayerActionDesc _desc)
    {
        if(isOwned)
        {
            if(ownerSc.gameObject.GetPhotonView().ViewID==_desc.playerObj.GetPhotonView().ViewID)
            {
                return true;
            }
            else
            {
                return false;
            }        
        }
        else
        {
            return true;
        }
    }

    public void CallPickUp(int _id)
    {
        photonView.RPC(nameof(PickUp), RpcTarget.AllBufferedViaServer, _id);
    }

    public void CallDump(int _id)
    {
        photonView.RPC(nameof(Dump), RpcTarget.AllBufferedViaServer, _id);
    }

    [PunRPC]
    public void PickUp(int _id)
    {
        if (!isOwned)
        {
            GameObject _obj = NetworkObjContainer.NetworkObjDictionary[_id];
            priority = 100;

            ownerSc = _obj.GetComponent<ItemPocket>();
            //Playerが二つ持っちゃう場合の例外処理
            if (ownerSc.gameObject.tag == "Player")
            {
                if (ownerSc.gameObject.transform.Find("Battery 1(Clone)") == null)
                {
                    ownerSc.SetItem(this.gameObject);
                    rb.isKinematic = true;
                    col.enabled = false;
                    this.transform.parent = _obj.transform;
                    //保有状態に切り替え
                    isOwned = true;
                }
            }
            else
            {
                ownerSc.SetItem(this.gameObject);
                rb.isKinematic = true;
                col.enabled = false;
                this.transform.parent = _obj.transform;

                //保有状態に切り替え
                isOwned = true;

            }
        }
    }

    [PunRPC]
    public void Dump(int _id)
    {
        if (isOwned)
        {
            GameObject _obj = NetworkObjContainer.NetworkObjDictionary[_id];
            if (_obj == ownerSc.gameObject)
            {
                ownerSc.SetItem(null);
                rb.isKinematic = false;
                col.enabled = true;
                this.transform.parent = null;

                isOwned = false;

                priority = 40;

                PlayerMove p_move = ownerSc.gameObject.GetComponent<PlayerMove>();
                if (p_move != null)
                {
                    if (_obj.GetPhotonView().IsMine)
                    {
                        playerDir = p_move.GetPlayerDir();
                        Vector3 _power = playerDir * throwForce;
                        _power.y = throwUpForce;
                        photonView.RPC(nameof(RPCThrowBattery), RpcTarget.All, _power);
                        if (ownerSc.gameObject.GetComponent<PlayerActionCtrl>().selectedActionObj == this.gameObject)
                            SimpleAudioManager.PlayOneShot(throwBatterrySe);
                    }
                }

                myControlUi.SetControlUIActive(false);
                ownerSc = null;
            }
        }
    }
    public float GetLevel()
    {
        return level;
    }

    public void BatteryConsumption(float _powerConsumption)
    {
        level -= _powerConsumption;
        //中のオブジェクトを残量に合わせて
        Vector3 keepSize = energyGazeObj.transform.localScale;
        //100は電池の最大容量のマジックナンバー
        keepSize.y = energyGazeSize * (level / 100f);

        energyGazeObj.transform.localScale = keepSize;
        if (level <= 0)
        {
            level = 0f;
            energyGazeObj.transform.localScale = Vector3.zero;
        }
    }

    [PunRPC]
    public void RPCSetBatteryLevel(float _level)
    {
        level = _level;
        //中のオブジェクトを残量に合わせて
        Vector3 keepSize = energyGazeObj.transform.localScale;
        //100は電池の最大容量のマジックナンバー
        keepSize.y = energyGazeSize * (level / 100f);

        energyGazeObj.transform.localScale = keepSize;
        if (level <= 0)
        {
            level = 0f;
            energyGazeObj.transform.localScale = Vector3.zero;
        }
    }

    [PunRPC]
    private void RPCThrowBattery(Vector3 _power)
    {
        rb.AddForce(_power, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //電池の所有者がいないのに所有されていることになっている場合の例外処理
        if(collision.gameObject.tag=="Player")
        {
            if(this.transform.parent==null)
            {
                isOwned = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Coal : MonoBehaviourPunCallbacks,IPlayerAction
{
    private Rigidbody rb;
    private BoxCollider col;
    // Start is called before the first frame update
    [SerializeField, ReadOnly]
    //保有されているか
    public bool isOwned;

    private BatteryHolder ownerSc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        if (!isOwned)
            PickUp(_desc);
        else
            Dump(_desc);
    }

    public void Dump(PlayerActionDesc _desc)
    {
        if (_desc.target == ownerSc.gameObject)
        {
            ownerSc.SetBattery(null);
            rb.isKinematic = false;
            // col.enabled = true;
            this.transform.parent = null;
            isOwned = false;
        }
    }

    private void PickUp(PlayerActionDesc _desc)
    {
        ownerSc = _desc.target.GetComponent<BatteryHolder>();
        ownerSc.SetBattery(this.gameObject);
        rb.isKinematic = true;
       // col.enabled = false;
        this.transform.parent = _desc.target.transform;
        //保有状態に切り替え
        isOwned = true;
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
}

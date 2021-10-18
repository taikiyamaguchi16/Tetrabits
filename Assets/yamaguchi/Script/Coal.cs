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

    private PlayerActionCtrl keepSc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        keepSc = _desc.target.GetComponent<PlayerActionCtrl>();
        Debug.Log(name + "実行");

        Debug.Log(name + "成功");
       // keepSc.ChangeHolding(this);

        rb.isKinematic = true;
        col.enabled = false;
        this.transform.parent = _desc.target.transform;
    }

    public void Dump()
    {
        this.transform.parent = null;
        rb.isKinematic = false;
        col.enabled = true;
        //keepSc.ChangeHolding(null);
    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
}

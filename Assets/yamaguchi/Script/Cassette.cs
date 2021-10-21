using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cassette : MonoBehaviour, IPlayerAction
{
    private Rigidbody rb;
    private BoxCollider col;
    // Start is called before the first frame update
    [SerializeField, ReadOnly]
    //保有されているか
    public bool isOwned;
    [SerializeField]
    private int priority;

    [SerializeField]
    SceneObject loadSceneObj;

    [SerializeField]
    bool isClear;

    private ItemPocket ownerSc;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<BoxCollider>();
        isOwned = false;
        isClear = false;
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {

        if (!isOwned)
        {
            PickUp(_desc.playerObj);
            //photonView.RPC(nameof(PickUp), RpcTarget.All);
        }
        else
            //photonView.RPC(nameof(Dump), RpcTarget.All, _desc.playerObj);
            Dump(_desc.playerObj);

    }
    public void EndPlayerAction(PlayerActionDesc _desc) { }
    public int GetPriority()
    {
        return priority;
    }

    public void Dump(GameObject _obj)
    {
        if (_obj == ownerSc.gameObject)
        {
            ownerSc.SetItem(null);  
            rb.isKinematic = false;
            col.enabled = true;
            this.transform.parent = null;
            isOwned = false;
            priority = 40;
        }
    }

    public void PickUp(GameObject _obj)
    {
        //持たれたとき用の角度
        this.transform.rotation = Quaternion.Euler(90f, 0f, 180f);

        priority = 100;
        ownerSc = _obj.GetComponent<ItemPocket>();
        ownerSc.SetItem(this.gameObject);
        rb.isKinematic = true;
        col.enabled = false;
        this.transform.parent = _obj.transform;
        //保有状態に切り替え
        isOwned = true;
    }

    public SceneObject GetLoadSceneObj()
    {
        return loadSceneObj;
    }

    public void SetIsClearOn()
    {
        isClear = true;
    }

    public bool GetIsClear()
    {
        return isClear;
    }
}

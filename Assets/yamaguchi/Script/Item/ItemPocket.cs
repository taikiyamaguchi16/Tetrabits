using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPocket : MonoBehaviour
{
    //保有しているオブジェクト
    private GameObject ownObj;
    [SerializeField]
    Transform pocketPos;

    //オブジェクトを保有しているかどうか
    public GameObject GetItem()
    {
        return ownObj;
    }
    public void SetItem(GameObject _item)
    {
        ownObj = _item;
        if (_item != null)
        {
            ownObj.transform.position = pocketPos.position;
            ownObj.transform.eulerAngles = pocketPos.eulerAngles;
        }
    }
    
}

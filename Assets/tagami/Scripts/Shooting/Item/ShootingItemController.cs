using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingItemController : MonoBehaviour
{
    [SerializeField] string itemId;

    // Update is called once per frame
    void Update()
    {
        //右に移動？
    }

    public bool CompareItemId(string _id)
    {
        return _id == itemId;
    }
}

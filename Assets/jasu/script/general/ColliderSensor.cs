using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSensor : MonoBehaviour
{
    int inColliderNum;   // オブジェクトのカウント

    [SerializeField]
    bool existInCollider = false; // コリジョン内に何かしらのオブジェクトがあればtrue;

    private void Start()
    {
        inColliderNum = 0;
    }

    private void Update()
    {
        if (inColliderNum > 0)
        {
            existInCollider = true;
        }
        else
        {
            existInCollider = false;
            inColliderNum = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inColliderNum++;
    }

    private void OnTriggerExit(Collider other)
    {
        inColliderNum--;
    }

    private void OnCollisionStay(Collision collision)
    {
        existInCollider = true;
    }

    public int GetInColliderNum()
    {
        return inColliderNum;
    }

    public bool GetExistInCollider()
    {
        return existInCollider;
    }
}

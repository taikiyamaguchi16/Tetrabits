using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSensor : MonoBehaviour
{
    [SerializeField]
    bool existInCollider = false; // コリジョン内に何かしらのオブジェクトがあればtrue;

    [SerializeField]
    float distanceToGround = 3f;

    [SerializeField]
    List<GameObject> exceptionList = new List<GameObject>();

    //public List<GameObject> objList = new List<GameObject>();

    private void Start()
    {
        //objList = new List<GameObject>();
    }

    private void Update()
    {
        //for (int i = 0; i < objList.Count; i++)
        //{
        //    if(objList[i] == null)
        //    {
        //        objList.Remove(objList[i]);
        //    }
        //}

        //if (objList.Count > 0)
        //{
        //    existInCollider = true;
        //}
        //else
        //{
        //    existInCollider = false;
        //    objList.Clear();
        //}

        Vector3 rayPosition = transform.position;
        Ray ray = new Ray(rayPosition, Vector3.down);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, distanceToGround))
        {
            foreach(var exception in exceptionList)
            {
                if(exception.GetInstanceID() == hitInfo.transform.gameObject.GetInstanceID())
                {
                    return;
                }
            }
            existInCollider = true;
            //Debug.Log("地面判定 : " + hitInfo.transform.gameObject.name);
        }
        else
        {
            existInCollider = false;
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    objList.Add(other.gameObject);
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    objList.Remove(other.gameObject);
    //}

    //private void OnCollisionEnter(Collision collision)
    //{
    //    objList.Add(collision.gameObject);
    //}

    //private void OnCollisionExit(Collision collision)
    //{
    //    objList.Remove(collision.gameObject);
    //}

    //public int GetInColliderNum()
    //{
    //    return objList.Count;
    //}

    public bool GetExistInCollider()
    {
        return existInCollider;
    }
}

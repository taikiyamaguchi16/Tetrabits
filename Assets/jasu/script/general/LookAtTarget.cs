using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField]
    Transform targetTrans = null;

    [SerializeField]
    bool lookAtX;

    [SerializeField]
    bool lookAtY;

    [SerializeField]
    bool lookAtZ;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTrans != null)
        {
            //Vector3 lookVec = targetTrans.position;

            //if (!lookAtX)
            //{
            //    lookVec.x = transform.position.x;
            //}

            //if (!lookAtY)
            //{
            //    lookVec.y = transform.position.y;
            //}

            //if (!lookAtZ)
            //{
            //    lookVec.z = transform.position.z;
            //}

            //transform.
        }
    }
}

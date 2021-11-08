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
            Vector3 lookVec = transform.rotation.eulerAngles;

            if (lookAtX)
            {
                lookVec.x = targetTrans.rotation.eulerAngles.x;
            }

            if (lookAtY)
            {
                lookVec.y = targetTrans.rotation.eulerAngles.y;
            }

            if (lookAtZ)
            {
                lookVec.z = targetTrans.rotation.eulerAngles.z;
            }

            transform.rotation = Quaternion.Euler(lookVec);
        }
    }
}

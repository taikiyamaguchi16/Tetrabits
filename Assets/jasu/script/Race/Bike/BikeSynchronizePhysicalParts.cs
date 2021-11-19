using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSynchronizePhysicalParts : MonoBehaviour
{
    [SerializeField]
    GameObject physicalParts;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 angle = transform.eulerAngles;
        //angle.z = -(physicalParts.transform.eulerAngles.x);
        //transform.eulerAngles = angle;

        //Vector3 angle = target.transform.localEulerAngles;
        //z = transform.localEulerAngles.x;
        //angle.z = -(transform.localEulerAngles.x);

        //target.transform.localEulerAngles = angle;
    }

    private void LateUpdate()
    {
        transform.localRotation = physicalParts.transform.localRotation;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FacingCameraXToZ : MonoBehaviour
{
    [SerializeField]
    GameObject target;

    // Update is called once per frame
    void Update()
    {
        Vector3 angle = transform.localRotation.eulerAngles;
        angle.z = target.transform.localRotation.eulerAngles.x;
        transform.localRotation = Quaternion.Euler(angle);
    }
}

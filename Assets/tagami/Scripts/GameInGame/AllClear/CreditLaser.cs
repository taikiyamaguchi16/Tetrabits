using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditLaser : MonoBehaviour
{
    [SerializeField] float laserForce = 1.0f;

    private void Update()
    {
        //パッド操作
        var endRotation = Quaternion.FromToRotation(Vector3.right, (TetraInput.sTetraPad.GetVector()).normalized);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, endRotation, 0.5f);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.attachedRigidbody?.AddForce(transform.right * laserForce);
    }

}

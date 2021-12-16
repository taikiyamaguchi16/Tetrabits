using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditLaser : MonoBehaviour
{
    [SerializeField] float laserForce = 1.0f;

    private void OnTriggerStay2D(Collider2D collision)
    {
        collision.attachedRigidbody?.AddForce(transform.right * laserForce);
    }

}

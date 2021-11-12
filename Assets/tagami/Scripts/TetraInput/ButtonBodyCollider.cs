using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBodyCollider : MonoBehaviour
{
    public int collisionNum { private set; get; } = 0;

    [System.NonSerialized]
    public bool triggerEnter;

    private void OnCollisionEnter(Collision collision)
    {
        collisionNum++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TetraInput"))
        {
            triggerEnter = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionNum--;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBodyCollider : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> collidingObjects;

    [System.NonSerialized]
    public bool triggerEnter;

    private void FixedUpdate()
    {
        collidingObjects.Clear();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            foreach (var obj in collidingObjects)
            {
                if (obj == collision.gameObject)
                {
                    return;
                }
            }

            collidingObjects.Add(collision.gameObject);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collidingObjects.Remove(collision.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("TetraInput"))
        {
            triggerEnter = true;
        }
    }
}

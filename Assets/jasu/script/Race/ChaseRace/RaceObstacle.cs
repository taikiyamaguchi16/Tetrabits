using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceObstacle : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    Vector3 force;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if (TetraInput.sTetraLever.GetPoweredOn())
            {
                rb.AddForce(force, ForceMode.Impulse);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
    }
}

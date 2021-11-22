using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitialVelocity2D : MonoBehaviour
{
    [SerializeField] Vector3 initialVelocity;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().velocity = initialVelocity;
    }
}

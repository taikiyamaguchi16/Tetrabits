using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierEnemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float rotationSpeed = 100.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, Vector3.forward);
    }
}

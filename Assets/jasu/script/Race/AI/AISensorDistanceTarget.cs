using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISensorDistanceTarget : AISensor
{
    [SerializeField]
    GameObject target;

    [SerializeField]
    float activeDistanceMax = 40f;

    [SerializeField]
    float activeDistanceMin = 10f;

    public float distanceToTarget { get; private set; }

    public float diffToTargetZ { get; private set; }

    // Update is called once per frame
    void Update()
    {
        diffToTargetZ = target.transform.position.z - transform.position.z;

        sensorActive = false;
        distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget < activeDistanceMax &&
            distanceToTarget > activeDistanceMin)
        {
            sensorActive = true;
        }
    }
}

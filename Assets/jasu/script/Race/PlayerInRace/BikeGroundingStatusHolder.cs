using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 使ってない
public class BikeGroundingStatusHolder : MonoBehaviour
{
    public enum BikeGroundingStatus
    {
        GroundingBoth = 0,
        GroundingFront,
        GroundingBack,
        NotGrandingBoth,
    }

    BikeGroundingStatus bikeGroundingStatus;

    [SerializeField]
    ColliderSensor colliderSensorFront = null;

    [SerializeField]
    ColliderSensor colliderSensorBack = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

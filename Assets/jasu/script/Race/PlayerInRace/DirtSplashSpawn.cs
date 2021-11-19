using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplashSpawn : MonoBehaviour
{
    [SerializeField]
    protected DirtSplashSpawner dirtSplashSpawner = null;

    [SerializeField]
    protected ColliderSensor colliderSensorFront = null;

    [SerializeField]
    protected ColliderSensor colliderSensorBack = null;

    [SerializeField]
    protected MoveInRace moveInRace;

    public Vector3 moveVec { get; set; }

    [SerializeField]
    protected float downSpdMultiply = 0.5f;

    public bool dirtSplashFlag { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if(colliderSensorFront.GetExistInCollider() ||
            colliderSensorFront.GetExistInCollider())
        {
            if (dirtSplashFlag)
            {
                dirtSplashFlag = false;
                dirtSplashSpawner.InstantiateDirtSplash(moveVec);
                Vector3 velocity = moveInRace.rb.velocity;
                velocity.z *= downSpdMultiply;
                moveInRace.rb.velocity = velocity;
            }
        }
    }
}

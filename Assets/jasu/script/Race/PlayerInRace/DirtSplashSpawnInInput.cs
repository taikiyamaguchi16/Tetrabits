using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplashSpawnInInput : MonoBehaviour
{
    [SerializeField]
    DirtSplashSpawner dirtSplashSpawner = null;

    [SerializeField]
    ColliderSensor colliderSensorFront = null;

    [SerializeField]
    ColliderSensor colliderSensorBack = null;

    [SerializeField]
    PlayerMoveInRace playerMoveInRace;

    public Vector3 playerMoveVec { get; set; }

    [SerializeField]
    float downSpdMultiply = 0.5f;

    // Update is called once per frame
    void Update()
    {
        if(colliderSensorFront.GetExistInCollider() ||
            colliderSensorFront.GetExistInCollider())
        {
            if (TetraInput.sTetraButton.GetTrigger())
            {
                dirtSplashSpawner.InstantiateDirtSplash(playerMoveVec);
                Vector3 playerVelocity = playerMoveInRace.rb.velocity;
                playerVelocity.z *= downSpdMultiply;
                playerMoveInRace.rb.velocity = playerVelocity;
            }
        }
    }
}

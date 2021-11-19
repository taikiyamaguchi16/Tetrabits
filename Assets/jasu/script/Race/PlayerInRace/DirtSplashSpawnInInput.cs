using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplashSpawnInInput : DirtSplashSpawn
{
    // Update is called once per frame
    void Update()
    {
        if(colliderSensorFront.GetExistInCollider() ||
            colliderSensorFront.GetExistInCollider())
        {
            if (TetraInput.sTetraButton.GetTrigger())
            {
                dirtSplashSpawner.InstantiateDirtSplash(moveVec);
                Vector3 velocity = moveInRace.rb.velocity;
                velocity.z *= downSpdMultiply;
                moveInRace.rb.velocity = velocity;
            }
        }
    }
}

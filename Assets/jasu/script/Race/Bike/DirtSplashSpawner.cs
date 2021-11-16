using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSplashSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject dirtSplashPrefab = null;
    

    public void InstantiateDirtSplash(Vector3 _moveForce)
    {
        GameObject dirtSplashObj = Instantiate(dirtSplashPrefab);
        dirtSplashObj.transform.position = transform.position;
        DirtSplash dirtSplash = dirtSplashObj.GetComponent<DirtSplash>();
        dirtSplash.parentInstanceID = gameObject.GetInstanceID();
        dirtSplash.parentMoveForce = _moveForce;
    }
}

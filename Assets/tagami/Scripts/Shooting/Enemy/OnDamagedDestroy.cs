using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamagedDestroy : MonoBehaviour, IShootingEnemy
{
    [Header("Option")]
    [SerializeField] List<GameObject> additionalDestroyObjects;

    public void OnDamaged()
    {
        Destroy(gameObject);

        foreach(var obj in additionalDestroyObjects)
        {
            Destroy(obj);
        }
    }
}

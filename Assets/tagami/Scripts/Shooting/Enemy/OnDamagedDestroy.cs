using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamagedDestroy : MonoBehaviour, IShootingEnemy
{
    [SerializeField] int hp = 1;

    [Header("Option")]
    [SerializeField] List<GameObject> additionalDestroyObjects;

    public void OnDamaged()
    {
        hp--;
        if (hp <= 0)
        {
            Destroy(gameObject);

            foreach (var obj in additionalDestroyObjects)
            {
                Destroy(obj);
            }
        }
    }
}

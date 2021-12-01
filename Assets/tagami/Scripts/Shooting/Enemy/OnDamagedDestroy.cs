using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDamagedDestroy : MonoBehaviour, IShootingEnemy
{
    [SerializeField] int hp = 1;

    [Header("Option")]

    [SerializeField] GameObject explosionPrefab;
    [SerializeField] List<GameObject> additionalDestroyObjects;

    public void OnDamaged()
    {
        hp--;
        if (hp <= 0)
        {
            if (explosionPrefab)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);

            foreach (var obj in additionalDestroyObjects)
            {
                Destroy(obj);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBullet : MonoBehaviour, IShootingEnemy
{
    [SerializeField] int hp = 5;

    [SerializeField] Sprite maxHpSprite;
    [SerializeField] Sprite halfHpSprite;

    public void OnDamaged()
    {
        hp--;

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        else if (hp < 3)
        {
            GetComponent<SpriteRenderer>().sprite = halfHpSprite;
        }

    }
}

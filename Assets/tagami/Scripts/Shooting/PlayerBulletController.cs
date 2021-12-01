using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerBulletController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //ダメージを与える
            var ishootingEnemys = collision.GetComponents<IShootingEnemy>();
            if (ishootingEnemys.Length > 0)
            {
                foreach (var ishootingEnemy in ishootingEnemys)
                {
                    ishootingEnemy.OnDamaged();
                }
            }

            //問答無用で自身を消去
            Destroy(gameObject);
        }
        if(collision.CompareTag("EnemyBullet"))
        {
            //ダメージを与える
            var ishootingEnemys = collision.GetComponents<IShootingEnemy>();
            if (ishootingEnemys.Length > 0)
            {
                foreach (var ishootingEnemy in ishootingEnemys)
                {
                    ishootingEnemy.OnDamaged();
                }

                //OnDamagedが実装されてるなら消去
                Destroy(gameObject);
            }
        }
    }
}

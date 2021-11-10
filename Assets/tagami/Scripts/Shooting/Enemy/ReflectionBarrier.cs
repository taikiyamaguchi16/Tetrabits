using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionBarrier : MonoBehaviour
{
    [SerializeField] GameObject enemyBulletPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //跳ね返す
            Rigidbody2D outRb = null;
            if (collision.TryGetComponent(out outRb))
            {
                //反転軸の作成
                var normal = (collision.transform.position - transform.position).normalized;
                //反転
                outRb.velocity = Vector3.Reflect(outRb.velocity, normal);
            }

        }
    }
}

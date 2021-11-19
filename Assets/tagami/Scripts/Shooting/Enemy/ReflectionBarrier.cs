using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionBarrier : MonoBehaviour
{
    [SerializeField] GameObject enemyBulletPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerBullet"))
        {
            //跳ね返す
            Rigidbody2D outRb = null;
            if (collision.TryGetComponent(out outRb))
            {
                //反転軸の作成
                var normal = (collision.transform.position - transform.position).normalized;
                //反転速度作成
                var reflectionVelocity = Vector3.Reflect(outRb.velocity, normal);

                if (Photon.Pun.PhotonNetwork.IsMasterClient)
                {
                    //敵弾生成
                    ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
                        enemyBulletPrefab.name, collision.transform.position, Quaternion.identity, reflectionVelocity);
                    //var obj=Instantiate(enemyBulletPrefab, collision.transform.position, Quaternion.identity);
                    //obj.GetComponent<Rigidbody2D>().velocity = reflectionVelocity;
                }

                //プレイヤーの弾削除
                Destroy(collision.gameObject);
            }

        }
    }
}

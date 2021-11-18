using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting
{
    public class FixedArtilleryEnemy : MonoBehaviour
    {
        [SerializeField] GameObject enemyBulletPrefab;
        [SerializeField] float bulletSpeed = 1.0f;
        [SerializeField] float shotIntervalSeconds = 1.0f;
        float shotIntervalTimer;

        // Update is called once per frame
        void Update()
        {
            shotIntervalTimer += Time.deltaTime;
            if (shotIntervalTimer >= shotIntervalSeconds)
            {
                shotIntervalTimer = 0.0f;

                var shootingPlayer = GameObject.Find("ShootingPlayer(Clone)");
                if (shootingPlayer && Photon.Pun.PhotonNetwork.IsMasterClient)
                {
                    //プレイヤーに向かって弾を撃つ
                    ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity
                        (enemyBulletPrefab.name, transform.position, Quaternion.identity,
                        (shootingPlayer.transform.position - transform.position).normalized * bulletSpeed);

                    //var bulletObj = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
                    //bulletObj.GetComponent<Rigidbody2D>().velocity = (shootingPlayer.transform.position - transform.position).normalized * bulletSpeed;
                }

                if(!shootingPlayer)
                {
                    Debug.LogWarning("Targetが見つからないため弾を生成できません");
                }

            }

        }
    }

}//namespace
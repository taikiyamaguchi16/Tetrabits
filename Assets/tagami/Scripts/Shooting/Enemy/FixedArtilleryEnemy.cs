using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedArtilleryEnemy : MonoBehaviour
{
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float bulletSpeed = 1.0f;
    [SerializeField] float shotIntervalSeconds = 1.0f;
    float shotIntervalTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        shotIntervalTimer += Time.deltaTime;
        if (shotIntervalTimer >= shotIntervalSeconds)
        {
            shotIntervalTimer = 0.0f;

            var shootingPlayer = GameObject.Find("ShootingPlayer");
            if (shootingPlayer)
            {
                //プレイヤーに向かって弾を撃つ
                var bulletObj = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
                bulletObj.GetComponent<Rigidbody2D>().velocity = (shootingPlayer.transform.position - transform.position).normalized * bulletSpeed;
            }

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotLeftEnemyBullet : MonoBehaviour
{
    [Header("Shot")]
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float bulletSpeed = 1.0f;
    [SerializeField] float shotIntervalSeconds = 1.0f;
    float shotIntervalTimer;

    // Update is called once per frame
    void Update()
    {
        //弾発射
        shotIntervalTimer += Time.deltaTime;
        if (shotIntervalTimer >= shotIntervalSeconds)
        {
            shotIntervalTimer = 0.0f;
            var bullet = Instantiate(enemyBulletPrefab, transform.position, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = -Vector2.right * bulletSpeed;
        }
    }
}

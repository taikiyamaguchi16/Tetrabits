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
    [SerializeField] bool randomShotIntervalTimer;

    void Start()
    {
        if (randomShotIntervalTimer)
        {
            shotIntervalTimer = Random.Range(0.0f, shotIntervalSeconds);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //弾発射
        shotIntervalTimer += Time.deltaTime;
        if (shotIntervalTimer >= shotIntervalSeconds)
        {
            shotIntervalTimer = 0.0f;

            if (Photon.Pun.PhotonNetwork.IsMasterClient)
            {
                ShootingGameManager.sShootingGameManager.CallLocalInstantiate(enemyBulletPrefab.name, transform.position, Quaternion.identity);              
            }
        }
    }
}

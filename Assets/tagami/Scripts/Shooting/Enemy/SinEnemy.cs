using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinEnemy : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeedX = 1.0f;
    [SerializeField] float moveSpeedY = 1.0f;
    [SerializeField] float verticalWidth = 1.0f;

    [Header("Shot")]
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float bulletSpeed = 1.0f;
    [SerializeField] float shotIntervalSeconds = 1.0f;
    float shotIntervalTimer;

    //位置
    float transformPositionX;
    float sinValue;

    // Start is called before the first frame update
    void Start()
    {
        transformPositionX = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        transformPositionX -= moveSpeedX * Time.deltaTime;
        sinValue += moveSpeedY * Time.deltaTime;
        transform.position = new Vector3(transformPositionX, Mathf.Sin(sinValue) * verticalWidth, 0.0f);

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

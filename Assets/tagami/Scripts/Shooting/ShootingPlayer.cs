using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShootingPlayer : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] float moveSpeed = 1.0f;

    [Header("Shot")]
    [SerializeField] GameObject playerBulletPrefab;
    [SerializeField] float shotIntervalSeconds = 1.0f;
    float shotIntervalTimer;

    Rigidbody2D myRb2D;

    // Start is called before the first frame update
    void Start()
    {
        myRb2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //移動
        myRb2D.velocity = TetraInput.sTetraPad.GetVector() * moveSpeed * Time.deltaTime;

        //弾発射処理
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            shotIntervalTimer += Time.deltaTime;
            if (shotIntervalTimer >= shotIntervalSeconds)
            {//発射
                shotIntervalTimer = 0.0f;

                //2wayとかに後で対応
                var bullet = Instantiate(playerBulletPrefab);
                bullet.transform.position = transform.position;
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemyタグオブジェクトにぶつかったので死にます");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Enemyタグオブジェクトにぶつかったので死にます");
        }
    }
}

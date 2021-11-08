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
    [SerializeField] float bulletSpeed = 10.0f;
    [SerializeField] float shotIntervalSeconds = 1.0f;
    float shotIntervalTimer;

    int shotLevel = 1;

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
                switch (shotLevel)
                {
                    case 1:
                        ShotBullet(Vector3.right * bulletSpeed);
                        break;
                    case 2:
                        ShotBullet((Vector3.right*5 + Vector3.up).normalized * bulletSpeed);
                        ShotBullet((Vector3.right*5 + Vector3.down).normalized * bulletSpeed);
                        break;
                    case 3:
                        ShotBullet((Vector3.right + Vector3.up).normalized * bulletSpeed);
                        ShotBullet(Vector3.right * 10.0f);
                        ShotBullet((Vector3.right + Vector3.down).normalized * bulletSpeed);
                        break;
                    default:
                        Debug.LogError("対応していないショットレベル：" + shotLevel);
                        break;
                }
            }

        }
    }

    private void ShotBullet(Vector3 _velocity)
    {
        var bullet = Instantiate(playerBulletPrefab, transform.position, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("Enemyタグオブジェクトにぶつかったので死にます");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("Enemyタグオブジェクトにぶつかったので死にます");
        }

        //レベルアップアイテム取得
        if (collision.gameObject.CompareTag("LevelUpItem"))
        {
            shotLevel++;
            if (shotLevel >= 3)
            {
                shotLevel = 3;
            }
            Destroy(collision.gameObject);
        }
    }
}

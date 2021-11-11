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

    [Header("Shot Level")]
    [SerializeField, Range(1, 3)] int shotLevel = 1;
    [SerializeField] float dualShotWidth = 1.0f;
    [SerializeField] float tripleShotWidth = 1.0f;

    [Header("Bomb")]
    [SerializeField] GameObject bombFieldPrefab;


    //damage    
    [Header("Damage")]
    [SerializeField] float invincibleSeconds = 1.0f;
    float invinvibleTimer;
    [SerializeField] Renderer flashingRenderer;
    [SerializeField] float flashingIntervalSeconds = 0.1f;
    float flashingIntervalTimer;
    bool isInvincible;
    [SerializeField] GameObject explosionPrefab;


    Rigidbody2D myRb2D;

    // Start is called before the first frame update
    void Start()
    {
        myRb2D = GetComponent<Rigidbody2D>();

        //無敵スタート
        isInvincible = true;
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
                        ShotBullet(Vector3.zero, Vector3.right * bulletSpeed);
                        break;
                    case 2:
                        ShotBullet(new Vector3(0.0f, dualShotWidth / 2, 0.0f), Vector3.right * bulletSpeed);
                        ShotBullet(new Vector3(0.0f, -dualShotWidth / 2, 0.0f), Vector3.right * bulletSpeed);
                        break;
                    case 3:
                        ShotBullet(Vector3.zero, (Vector3.right + Vector3.up * tripleShotWidth).normalized * bulletSpeed);
                        ShotBullet(Vector3.zero, Vector3.right * bulletSpeed);
                        ShotBullet(Vector3.zero, (Vector3.right + Vector3.down * tripleShotWidth).normalized * bulletSpeed);
                        break;
                    default:
                        Debug.LogError("対応していないショットレベル：" + shotLevel);
                        break;
                }
            }

        }//lever on

        if (TetraInput.sTetraButton.GetTrigger() && ShootingGameManager.sShootingGameManager.bombNum > 0)
        { //ボム
            ShootingGameManager.sShootingGameManager.AddBomb(-1);

            //敵の弾をすべて粉砕する
            //foreach (var obj in GameObject.FindGameObjectsWithTag("EnemyBullet"))
            //{
            //    Destroy(obj);
            //}
            Instantiate(bombFieldPrefab, transform.position, Quaternion.identity);
        
        }

        if (isInvincible)
        {
            invinvibleTimer += Time.deltaTime;
            if (invinvibleTimer >= invincibleSeconds)
            {//無敵終了
                invinvibleTimer = 0.0f;
                isInvincible = false;
                flashingRenderer.enabled = true;
            }
            else
            {
                flashingIntervalTimer += Time.deltaTime;
                if (flashingIntervalTimer >= flashingIntervalSeconds)
                {
                    flashingIntervalTimer = 0.0f;
                    //切り替え
                    flashingRenderer.enabled = !flashingRenderer.enabled;
                }
            }
        }//無敵中処理
    }

    private void ShotBullet(Vector3 _offset, Vector3 _velocity)
    {
        var bullet = Instantiate(playerBulletPrefab, transform.position + _offset, Quaternion.identity);
        bullet.GetComponent<Rigidbody2D>().velocity = _velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            DealDamage();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
        {
            DealDamage();
        }

        //アイテム取得
        if (collision.gameObject.CompareTag("LevelUpItem"))
        {
            ShootingItemController item;
            if (collision.TryGetComponent(out item))
            {
                if (item.CompareItemId("levelup"))
                {
                    shotLevel++;
                    if (shotLevel >= 3)
                    {
                        shotLevel = 3;
                    }
                }
                else if (item.CompareItemId("bomb"))
                {
                    ShootingGameManager.sShootingGameManager.AddBomb(1);
                }

                Destroy(collision.gameObject);
            }
        }
    }

    private void DealDamage()
    {
        //無敵なので即終了
        if (isInvincible) { return; }


        //レベル２以上ならレベルを一つ下げて一時無敵に
        if (shotLevel >= 2)
        {
            shotLevel--;
            isInvincible = true;
            MonitorManager.DealDamageToMonitor("small");
        }
        else
        {
            //爆発
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            //一機減らす
            ShootingGameManager.sShootingGameManager.DestroyedPlayer(transform.position);

            //破壊
            Destroy(gameObject);
        }
        
        Debug.Log("EnemyタグorEnemyBulletタグオブジェクトにぶつかった");
    }
}

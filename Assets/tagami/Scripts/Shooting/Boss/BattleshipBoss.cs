using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipBoss : MonoBehaviour, IShootingEnemy
{


    [Header("Status")]
    bool movable;
    [SerializeField] float moveSpeed = 1.0f;
    [SerializeField] int hpMax = 100;
    int hp;

    [Header("Dead")]
    bool isDead;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] ShootingCameraStopper bothDeadCameraStopper;

    [Header("Shot")]
    [SerializeField] GameObject enemyBulletPrefab;
    [SerializeField] float bulletSpeed = 1.0f;
    float shotIntervalTimer;
    [SerializeField] float shotIntervalSeconds = 1.0f;
    [SerializeField] float bulletWidth = 1.0f;
    bool delayShooting;

    GameObject playerObject;

    // Start is called before the first frame update
    void Start()
    {
        movable = true;
        hp = hpMax;

        if(!bothDeadCameraStopper)
        {
            Debug.LogError("Scene上のShootingCameraStopperを設定してください");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;

        if (!playerObject)
        {
            playerObject = GameObject.Find("ShootingPlayer(Clone)");
        }

        var moveY = moveSpeed * Time.deltaTime;
        if (playerObject)
        {
            if (!delayShooting)
            {
                shotIntervalTimer += Time.deltaTime;
                if (shotIntervalTimer > shotIntervalSeconds)
                {
                    shotIntervalTimer = 0.0f;
                    //発射
                    StartCoroutine(DelayShot(0.5f));
                }
            }

            if (movable)
            {
                if (transform.position.y > playerObject.transform.position.y + moveY)
                {
                    //playerより上にいる
                    transform.position += new Vector3(0.0f, -moveY, 0.0f);
                }
                else if (transform.position.y < playerObject.transform.position.y - moveY)
                {
                    transform.position += new Vector3(0.0f, moveY, 0.0f);
                }
            }
        }
    }

    public IEnumerator DelayShot(float _waitTime)
    {
        delayShooting = true;
        movable = false;

        yield return new WaitForSeconds(_waitTime);

        //発射
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            Debug.Log("発射！");
            ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
                enemyBulletPrefab.name, transform.position, Quaternion.identity, -Vector3.right * bulletSpeed);

            ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
               enemyBulletPrefab.name, transform.position + new Vector3(0.0f, bulletWidth, 0.0f), Quaternion.identity, -Vector3.right * bulletSpeed);

            ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
               enemyBulletPrefab.name, transform.position + new Vector3(0.0f, -bulletWidth, 0.0f), Quaternion.identity, -Vector3.right * bulletSpeed);
        }

        //何秒か待つ
        yield return new WaitForSeconds(_waitTime);

        movable = true;
        delayShooting = false;
    }

    public void OnDamaged()
    {
        hp--;
        if (hp <= 0 && !isDead)
        {
            isDead = true;
            StartCoroutine(DeadAction());
        }
    }

    IEnumerator DeadAction()
    {
        //まずは爆破
        int numDeadExplosion = 10;
        for (int i = 0; i < numDeadExplosion; i++)
        {
            var rand = Random.insideUnitCircle * 2.0f;
            Instantiate(explosionPrefab, transform.position + new Vector3(rand.x, rand.y, 0.0f), Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        //下へ落ちていく
        float timer = 0.0f;
        float initialLocalPositionX = transform.localPosition.x;
        while (true)
        {
            timer += Time.deltaTime;
            //Xブルブル
            transform.localPosition = new Vector3(initialLocalPositionX + Mathf.Sin(timer * 30.0f) * 0.1f
                , transform.localPosition.y, transform.localPosition.z);

            //下へ
            transform.localPosition -= new Vector3(0.0f, 4.0f, 0.0f) * Time.deltaTime;
            if (transform.localPosition.y <= -8.0f)
            {
                break;
            }
            yield return null;
        }

        //ゲームクリア
        //GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;

        //カメラを停止させているのを破壊
        Destroy(bothDeadCameraStopper.gameObject);
        //自身を消去
        Destroy(gameObject);
    }


}

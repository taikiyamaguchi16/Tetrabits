using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipBoss : MonoBehaviour, IShootingEnemy
{
    [Header("Status")]
    bool movable;
    [SerializeField] float moveSpeed = 1.0f;


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
    }

    // Update is called once per frame
    void Update()
    {
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
        Debug.Log("発射！");
        ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
            enemyBulletPrefab.name, transform.position, Quaternion.identity, -Vector3.right * bulletSpeed);

        ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
           enemyBulletPrefab.name, transform.position + new Vector3(0.0f, bulletWidth, 0.0f), Quaternion.identity, -Vector3.right * bulletSpeed);


        ShootingGameManager.sShootingGameManager.CallLocalInstantiateWithVelocity(
           enemyBulletPrefab.name, transform.position + new Vector3(0.0f, -bulletWidth, 0.0f), Quaternion.identity, -Vector3.right * bulletSpeed);

        //何秒か待つ
        yield return new WaitForSeconds(_waitTime);

        movable = true;
        delayShooting = false;
    }

    public void OnDamaged()
    {

    }
}

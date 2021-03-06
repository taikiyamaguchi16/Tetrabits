using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;

public class JetController : MonoBehaviourPunCallbacks
{
    [Header("Reference")]
    [SerializeField] GameObject bulletPrefab;

    // [SerializeField] SceneObject nextScene;

    [Header("Status")]
    [SerializeField] float moveSpeed = 1.0f;

    [SerializeField] float shotIntervalTime = 1.0f;
    float shotTimer;

    float selfDestroyTimer;
    [SerializeField] float selfDestroySeconds = 10.0f;
    [SerializeField] Slider destroyGaugeSlider;
    [SerializeField] float bombSelfDestroyCostSeconds = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        shotTimer = Random.Range(0.0f, shotIntervalTime);

        GameInGameUtil.StartGameInGameTimer("shooting");
    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.R))
        //{
        //    GameInGameUtil.SwitchGameInGameScene(nextScene);
        //}


        //**********************************************************
        //ボタン
        if (TetraInput.sTetraButton.GetTrigger())
        {
            Debug.Log("Bomb!");
            foreach (var obj in GameObject.FindGameObjectsWithTag("Bullet"))
            {
                Destroy(obj);
            }
            selfDestroyTimer += bombSelfDestroyCostSeconds;

            GameInGameUtil.PlayCrackers();
        }

        //**********************************************************
        //レバー
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            if (PhotonNetwork.IsMasterClient)
            {
                //弾発射
                shotTimer += Time.deltaTime;
                if (shotTimer >= shotIntervalTime)
                {
                    shotTimer = 0.0f;
                    //発射
                    CallShotPlayerBullet();
                    //var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                    //obj.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10.0f;
                    //GameInGameUtil.MoveGameObjectToOwnerScene(obj, gameObject);
                }

                //自爆のゲージためる
                selfDestroyTimer += Time.deltaTime;
            }
        }
        else
        {
            shotTimer = 0.0f;

            selfDestroyTimer -= Time.deltaTime;
            if (selfDestroyTimer <= 0)
            {
                selfDestroyTimer = 0;
            }
        }

        //**********************************************************
        //SelfDestroy
        if (selfDestroyTimer >= selfDestroySeconds)
        {
            //MonitorManager.DealDamageToMonitor(3);
            selfDestroyTimer = 0.0f;//reset
        }
        //UpdateGauge
        destroyGaugeSlider.value = (selfDestroyTimer / selfDestroySeconds) * 100;

        //**********************************************************
        //Move

        if (PhotonNetwork.IsMasterClient)
        {
            Vector3 moveVec = Vector3.zero;
            moveVec.x += TetraInput.sTetraPad.GetVector().x;
            moveVec.y += TetraInput.sTetraPad.GetVector().y;
            transform.position += moveVec * moveSpeed * Time.deltaTime;
        }
    }


    private void CallShotPlayerBullet()
    {
        photonView.RPC(nameof(RPCShotPlayerBullet), RpcTarget.AllViaServer);
    }
    [PunRPC]
    public void RPCShotPlayerBullet()
    {
        var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
        obj.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10.0f;
        GameInGameUtil.MoveGameObjectToOwnerScene(obj, gameObject);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JetController : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject bulletPrefab;

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

    }

    // Update is called once per frame
    void Update()
    {
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
        }

        //**********************************************************
        //レバー
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            //弾発射
            shotTimer += Time.deltaTime;
            if (shotTimer >= shotIntervalTime)
            {
                shotTimer = 0.0f;
                //発射
                var obj = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                obj.GetComponent<Rigidbody2D>().velocity = Vector2.right * 10.0f;
                GameInGameUtil.MoveGameObjectToOwnerScene(obj, gameObject);
                
            }

            //自爆のゲージためる
            selfDestroyTimer += Time.deltaTime;
            if (selfDestroyTimer >= selfDestroySeconds)
            {
                selfDestroyTimer = selfDestroySeconds;
                Destroy(gameObject);
            }
        }
        else
        {
            shotTimer = 0.0f;

            selfDestroyTimer -= Time.deltaTime;
            if(selfDestroyTimer<=0)
            {
                selfDestroyTimer = 0;
            }
        }

        //**********************************************************
        //SelfDestroy
        if (selfDestroyTimer >= selfDestroySeconds)
        {
            selfDestroyTimer = selfDestroySeconds;
            Destroy(gameObject);
        }
        //UpdateGauge
        destroyGaugeSlider.value = (selfDestroyTimer / selfDestroySeconds)*100;

        //**********************************************************
        //Move
        Vector3 moveVec = Vector3.zero;
        moveVec.x += TetraInput.sTetraPad.GetVector().x;
        moveVec.y += TetraInput.sTetraPad.GetVector().y;
        transform.position += moveVec * moveSpeed * Time.deltaTime;
    }
}

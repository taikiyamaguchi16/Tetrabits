using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerDemo : DOManager
{
    //[SerializeField]
    //[Header("-X方向に動く目標値")]
    GameObject LeftGoal;

    //[SerializeField]
    //[Header("X方向に動く目標値")]
    GameObject RightGoal;

    // DemoManager
    GameObject DemoManager;

    [SerializeField]
    [Header("Z方向の目標値")]
    Vector3 ZGoal;

    [SerializeField]
    [Header("移動秒数最小")]
    float moveTimeMin = 4.0f;

    [SerializeField]
    [Header("移動秒数大")]
    float moveTimeMax = 10.0f;

    [SerializeField]
    [Header("移動秒数最小")]
    float scaleTimeMin = 4.0f;

    [SerializeField]
    [Header("移動秒数最小")]
    float scaleTimeMax = 10.0f;

    [SerializeField]
    [Header("Z方向のプレイヤーならtrue")]
    bool IamZ = false;

    bool isLeft = false;
    bool isRight = false;

    // スプライト取得用
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        LeftGoal = GameObject.Find("PlayerSpawnerLeft");
        RightGoal = GameObject.Find("PlayerSpawnerRight");
        DemoManager = GameObject.Find("DemoManager");

        sprite = gameObject.GetComponent<SpriteRenderer>();

        // Z移動の子のために初期スケール0
        gameObject.transform.localScale = Vector3.zero;

        if(gameObject.transform.position.x < RightGoal.transform.position.x && !IamZ)
        {
            // サイズ指定
            gameObject.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
            gameObject.GetComponent<SpriteRenderer>().flipX = true;
            isLeft = true;
        }
        else if(gameObject.transform.position.x > LeftGoal.transform.position.x && !IamZ)
        {
            // サイズ指定
            gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            gameObject.GetComponent<SpriteRenderer>().flipX = false;
            isRight = true;
        }

        // 移動させる速度(開始から終着点まで移動する秒数)を決定
        moveTime = Random.Range(moveTimeMin, moveTimeMax);

        //スケールさせる速度
        scaleTime = Random.Range(scaleTimeMin, scaleTimeMax);

        // Z方向の子
        if (IamZ)
        {
            // スケールを目標値まで変化させたのち破棄
            gameObject.transform.DOScale(ZGoal, scaleTime).SetEase(easeTypes).OnComplete(() =>
            {
                sprite.DOFade(fadeRange, fadeTime).SetEase(easeTypes).OnComplete(() => 
                {
                    Destroy(gameObject);
                    DemoManager.GetComponent<TitleDemoManager>().PlayerCountDown();
                });
            });
        }
        else
        {
            // 右に行く人
            if (isLeft)
            {
                moveRange = RightGoal.transform.position;

                gameObject.transform.DOMove(moveRange, moveTime).SetEase(easeTypes).OnComplete(() =>
                {
                    Destroy(gameObject);
                    DemoManager.GetComponent<TitleDemoManager>().PlayerCountDown();
                });
            }
            // 左に行く人
            else if (isRight)
            {
                moveRange = LeftGoal.transform.position;

                gameObject.transform.DOMove(moveRange, moveTime).SetEase(easeTypes).OnComplete(() =>
                {
                    Destroy(gameObject);
                    DemoManager.GetComponent<TitleDemoManager>().PlayerCountDown();
                });
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

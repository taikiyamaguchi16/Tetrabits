using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingGameManager : MonoBehaviour
{
    //残機
    [Header("Player")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] int lifeMax = 3;

    public int life { private set; get; }

    private void Awake()
    {
        if (!sShootingGameManager)
        {
            sShootingGameManager = this;
            //初期設定を行う
            life = lifeMax;

            //破壊されない設定にする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("すでに管理者が存在していたため、このオブジェクトを削除します");
            Destroy(gameObject);
        }

    }
    // Update is called once per frame
    void Update()
    {
        //クリアもしくはゲームオーバー処理
        if (false)
        {
            //管理者をなくす
            sShootingGameManager = null;
            Destroy(gameObject);
        }
    }

    public void GameClear()
    {

    }

    public void MinusLife(Vector3 _destroyedPosition)
    {
        //残機消費
        life--;
        if (life <= 0)
        {
            //ゲームオーバー


        }
        else
        {

            //player再出現の処理準備

            //カメラの動きを止めておく
        }
    }


    //**********************************************************
    //static 
    public static ShootingGameManager sShootingGameManager { private set; get; } = null;

}

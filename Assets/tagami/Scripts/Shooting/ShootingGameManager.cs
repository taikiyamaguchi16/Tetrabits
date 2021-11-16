using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class ShootingGameManager : MonoBehaviour
{

    [Header("Prefab Reference")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] ShootingCamera shootingCamera;

    //残機
    [Header("Player")]
    [SerializeField] int lifeMax = 3;
    public int life { private set; get; }

    [Header("Bomb")]
    [SerializeField] int initialBombNum = 3;
    //ボム
    public int bombNum { private set; get; } = 0;

    //再開処理
    [Header("Restart")]
    [SerializeField] float restartSeconds = 1.0f;
    float restartTimer;
    bool restart;
    Vector3 destroyedPlayerPosition;

    private void Awake()
    {
        if (!sShootingGameManager)
        {
            sShootingGameManager = this;
            //初期設定を行う
            life = lifeMax;
            bombNum = initialBombNum;
            destroyedPlayerPosition = transform.position;

            //破壊されない設定にする
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("すでに管理者が存在していたため、このオブジェクトを削除します");
            Destroy(gameObject);
        }

    }

    private void Start()
    {
        InstantiatePlayer();
        
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

        //再開処理
        if (restart)
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartSeconds)
            {
                restart = false;
                restartTimer = 0.0f;
                //再開
                InstantiatePlayer();
                shootingCamera.enabled = true;
            }
        }
    }

    public void StageClear()
    {
        //UI表示

        //最終ステージなら終了

    }

    public void DestroyedPlayer(Vector3 _destroyedPosition)
    {
        //残機消費
        Debug.Log(_destroyedPosition + ":でShootingPlayerが爆散しました");

        life--;
        destroyedPlayerPosition = _destroyedPosition;

        if (life <= 0)
        {
            //ゲームオーバーUI表示
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor("large");
            }
        }
        else
        {
            //player再出現の処理準備
            restart = true;
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor("medium");
            }
        }

        //カメラの動きを止めておく
        shootingCamera.enabled = false;
    }

    public void AddBomb(int _num)
    {
        bombNum += _num;
    }


    //private
    private void InstantiatePlayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.InstantiateRoomObject("Shooting/Player/" + playerPrefab.name, destroyedPlayerPosition, Quaternion.identity);
        }
    }


    //**********************************************************
    //static 
    public static ShootingGameManager sShootingGameManager { private set; get; } = null;

}

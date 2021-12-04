using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

public class ShootingGameManager : MonoBehaviourPunCallbacks
{

    [Header("Prefab Reference")]
    [SerializeField] GameObject playerPrefab;
    [SerializeField] ShootingCamera shootingCamera;

    //残機
    [Header("Player")]
    [SerializeField] int lifeMax = 10;
    public int life { private set; get; }

    [Header("Bomb")]
    [SerializeField] int initialBombNum = 3;
    public int bomb { private set; get; }

    //再開処理
    [Header("Restart")]
    [SerializeField] Trisibo.SceneField restartScene;
    [SerializeField] float restartSeconds = 1.0f;
    float restartTimer;
    bool restart;
    Vector3 destroyedPlayerPosition;

    [Header("Game Clear Over")]
    [SerializeField] Trisibo.SceneField nextScene;

    [Header("Local Instantiate")]
    [SerializeField] List<GameObject> localInstantiatePrefabs;

    //static 持ち越し要素
    //static bool sInitialized = false;
    //public static int sBombNum { private set; get; }
    //public static int sLife { private set; get; }

    private void Awake()
    {
        //マネージャー上書き
        sShootingGameManager = this;
        destroyedPlayerPosition = transform.position;

        life = lifeMax;
        bomb = initialBombNum;

        //static
        //if (!sInitialized)
        //{
        //    //初期設定を行う
        //    sLife = lifeMax;
        //    sBombNum = initialBombNum;
        //    sInitialized = true;
        //}
    }

    private void Start()
    {
        if (restartScene == null || restartScene.BuildIndex < 0)
        {
            Debug.LogError("再出撃の際のSceneが設定されていません");
        }

        InstantiatePlayer();
    }

    // Update is called once per frame
    void Update()
    {
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
        if (nextScene == null || nextScene.BuildIndex < 0)
        {
            Debug.Log("Shooting最終ステージクリア 全ステージクリアにより強制ダウンを行います");
            GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
            //sInitialized = false;
        }
        else
        {
            //次のステージへ移行します
            Debug.Log("ShootingStageをクリアしました　次のステージへ遷移します");
            Debug.Log("BuildIndex:" + nextScene.BuildIndex);
            if (PhotonNetwork.IsMasterClient)
            {
                GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(nextScene.BuildIndex));
            }
        }
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

                //ステージの最初に戻る
                //sInitialized = false;   //設定リセット
                GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(restartScene.BuildIndex));
            }
        }
        else
        {
            //player再出現の処理準備
            restart = true;
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor("small");
            }
        }

        //カメラの動きを止めておく
        if(shootingCamera)
        shootingCamera.enabled = false;
    }

    public void AddBomb(int _num)
    {
        bomb += _num;
    }

    public void AddLife(int _num)
    {
        life += _num;
    }

    public void CallLocalInstantiateWithVelocity(string _prefabName, Vector3 _position, Quaternion _rotation, Vector3 _velocity)
    {
        photonView.RPC(nameof(RPCLocalInstantiateWithVelocity), RpcTarget.AllViaServer, _prefabName, _position, _rotation, _velocity);
    }
    [PunRPC]
    public void RPCLocalInstantiateWithVelocity(string _prefabName, Vector3 _position, Quaternion _rotation, Vector3 _velocity)
    {
        foreach (var prefab in localInstantiatePrefabs)
        {
            if (prefab.name == _prefabName)
            {
                var obj = Instantiate(prefab, _position, _rotation);
                obj.GetComponent<Rigidbody2D>().velocity = _velocity;
                return;
            }
        }

        Debug.LogError("アタッチされていないPrefabを生成しようとしました");
    }

    public void CallLocalInstantiate(string _prefabName, Vector3 _position, Quaternion _rotation)
    {
        photonView.RPC(nameof(RPCLocalInstantiate), RpcTarget.AllViaServer, _prefabName, _position, _rotation);
    }
    [PunRPC]
    public void RPCLocalInstantiate(string _prefabName, Vector3 _position, Quaternion _rotation)
    {
        foreach (var obj in localInstantiatePrefabs)
        {
            if (obj.name == _prefabName)
            {
                Instantiate(obj, _position, _rotation);
                return;
            }
        }

        Debug.LogError("アタッチされていないPrefabを生成しようとしました");
    }

    //private
    private void InstantiatePlayer()
    {
        //Debug.Log("isMasterClient:" + PhotonNetwork.IsMasterClient);

        if (PhotonNetwork.IsMasterClient)
        {
            //Debug.Log("生成処理を呼びました");
            PhotonNetwork.InstantiateRoomObject("Shooting/Player/" + playerPrefab.name, destroyedPlayerPosition, Quaternion.identity);
        }
    }


    //**********************************************************
    //static 
    public static ShootingGameManager sShootingGameManager { private set; get; } = null;

}

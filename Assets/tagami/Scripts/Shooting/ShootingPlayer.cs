using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;

namespace Shooting
{

    [RequireComponent(typeof(Rigidbody2D))]
    public class ShootingPlayer : MonoBehaviourPunCallbacks
    {
        [Header("Move")]
        [SerializeField] float moveSpeed = 1.0f;
        [SerializeField] Vector2 limitArea = Vector2.one;

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
        bool isDead;

        //Photon
        PhotonTransformViewClassic photonTransformView;


        //Rb
        Rigidbody2D myRb2D;

        // Start is called before the first frame update
        void Start()
        {
            myRb2D = GetComponent<Rigidbody2D>();
            photonTransformView = GetComponent<PhotonTransformViewClassic>();

            //無敵スタート
            isInvincible = true;
        }

        // Update is called once per frame
        void Update()
        {
            //親子関係を作る
            //本来はStart()で行いたかったが、１フレーム待たなければいけないことからここへ記入
            if (!transform.parent)
            {
                var managerObj = GameObject.Find("ShootingGameManager");
                if (managerObj)
                {
                    transform.parent = managerObj.transform;
                }
            }

            //移動

            if (PhotonNetwork.IsMasterClient)
            {
                myRb2D.velocity = TetraInput.sTetraPad.GetVector() * moveSpeed * Time.deltaTime;
                photonTransformView.SetSynchronizedValues(myRb2D.velocity, 0);
            }


            //弾発射処理
            if (PhotonNetwork.IsMasterClient && TetraInput.sTetraLever.GetPoweredOn())
            {
                shotIntervalTimer += Time.deltaTime;
                if (shotIntervalTimer >= shotIntervalSeconds)
                {//発射
                    shotIntervalTimer = 0.0f;

                    //仮変更11/18　四捨五入してみる
                    //int shotLevelOnPad = (int)TetraInput.sTetraPad.GetVector().magnitude;
                    //if (TetraInput.sTetraPad.GetVector().magnitude - shotLevelOnPad >= 0.5f)
                    //{
                    //    shotLevelOnPad++;
                    //}

                    switch (TetraInput.sTetraPad.GetNumOnPad())
                    {
                        case 0:
                            //何もしない
                            break;
                        case 1:
                            CallShotBullet(Vector3.zero, Vector3.right * bulletSpeed);
                            break;
                        case 2:
                            CallShotBullet(new Vector3(0.0f, dualShotWidth / 2, 0.0f), Vector3.right * bulletSpeed);
                            CallShotBullet(new Vector3(0.0f, -dualShotWidth / 2, 0.0f), Vector3.right * bulletSpeed);
                            break;
                        case 3:
                            CallShotBullet(Vector3.zero, (Vector3.right + Vector3.up * tripleShotWidth).normalized * bulletSpeed);
                            CallShotBullet(Vector3.zero, Vector3.right * bulletSpeed);
                            CallShotBullet(Vector3.zero, (Vector3.right + Vector3.down * tripleShotWidth).normalized * bulletSpeed);
                            break;
                        case 4:
                            CallShotBullet(Vector3.zero, (Vector3.right + Vector3.up * tripleShotWidth).normalized * bulletSpeed);
                            CallShotBullet(new Vector3(0.0f, dualShotWidth / 2, 0.0f), Vector3.right * bulletSpeed);
                            CallShotBullet(new Vector3(0.0f, -dualShotWidth / 2, 0.0f), Vector3.right * bulletSpeed);
                            CallShotBullet(Vector3.zero, (Vector3.right + Vector3.down * tripleShotWidth).normalized * bulletSpeed);
                            break;
                        default:
                            Debug.LogWarning("対応していないショットレベル：" + shotLevel);
                            break;
                    }
                }
            }//lever on

            if (PhotonNetwork.IsMasterClient
                && TetraInput.sTetraButton.GetTrigger()
                && ShootingGameManager.sBombNum > 0)
            { //ボム
                CallInstantiateBombLocal();                
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

            //削除処理
            if (PhotonNetwork.IsMasterClient && isDead)
            {
                PhotonNetwork.Destroy(gameObject);
            }

            //範囲制限
            Vector3 localPosition = transform.localPosition;
            if (localPosition.x >= limitArea.x)
            {
                localPosition.x = limitArea.x;
            }
            if (localPosition.x <= -limitArea.x)
            {
                localPosition.x = -limitArea.x;
            }
            if (localPosition.y >= limitArea.y)
            {
                localPosition.y = limitArea.y;
            }
            if (localPosition.y <= -limitArea.y)
            {
                localPosition.y = -limitArea.y;
            }
            transform.localPosition = localPosition;

        }

        private void CallInstantiateBombLocal()
        {
            photonView.RPC(nameof(RPCInstantiateBombLocal), RpcTarget.All);
        }
        [PunRPC]
        public void RPCInstantiateBombLocal()
        {
            ShootingGameManager.sShootingGameManager.AddBomb(-1);
            var bombObj = Instantiate(bombFieldPrefab, transform.position, Quaternion.identity);
            bombObj.GetComponent<TransformSynchronizer>().targetObject = gameObject;
        }


        public void CallShotBullet(Vector3 _offset, Vector3 _velocity)
        {
            photonView.RPC(nameof(RPCShotBullet), RpcTarget.AllViaServer, _offset, _velocity);
        }
        [PunRPC]
        public void RPCShotBullet(Vector3 _offset, Vector3 _velocity)
        {
            var bullet = Instantiate(playerBulletPrefab, transform.position + _offset, Quaternion.identity);
            bullet.GetComponent<Rigidbody2D>().velocity = _velocity;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!PhotonNetwork.IsMasterClient) return;

            if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
            {
                if (!isInvincible)
                {
                    CallDealDamageToPlayer();
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("EnemyBullet"))
                {
                    if (!isInvincible)
                    {
                        CallDealDamageToPlayer();
                    }
                }
            }

            //アイテム取得
            if (collision.gameObject.CompareTag("LevelUpItem"))
            {
                ShootingItemController item;
                if (PhotonNetwork.IsMasterClient && collision.TryGetComponent(out item))
                {
                    if (item.CompareItemId("levelup"))
                    {
                        CallShotLevelUp();
                    }
                    else if (item.CompareItemId("bomb"))
                    {
                        CallAddBomb();
                    }
                }

                Destroy(collision.gameObject);
            }
        }

        private void CallAddBomb()
        {
            photonView.RPC(nameof(RPCAddBomb), RpcTarget.All);
        }
        [PunRPC]
        public void RPCAddBomb()
        {
            ShootingGameManager.sShootingGameManager.AddBomb(1);
        }

        private void CallShotLevelUp()
        {
            photonView.RPC(nameof(RPCShotLevelUp), RpcTarget.AllViaServer);
        }
        [PunRPC]
        public void RPCShotLevelUp()
        {
            shotLevel++;
            if (shotLevel >= 3)
            {
                shotLevel = 3;
            }
        }

        private void CallDealDamageToPlayer()
        {
            photonView.RPC(nameof(RPCDealDamageToPlayer), RpcTarget.All);
        }
        [PunRPC]
        public void RPCDealDamageToPlayer()
        {
            Debug.Log("RPCDealDamageToPlayer");

            //無敵判定にする
            isInvincible = true;

            //レベル２以上ならレベルを一つ下げて一時無敵に
            if (shotLevel >= 2)
            {
                shotLevel--;

                if (PhotonNetwork.IsMasterClient)
                {
                    MonitorManager.DealDamageToMonitor("small");
                }
            }
            else
            {
                //爆発
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);

                //一機減らす
                ShootingGameManager.sShootingGameManager.DestroyedPlayer(transform.position);

                //破壊
                isDead = true;
            }
        }
    }

}//namespace
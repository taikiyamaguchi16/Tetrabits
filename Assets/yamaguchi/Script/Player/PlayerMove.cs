using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public struct MoveSpeedInRatio
{
    [Tooltip("割合(0~1)")] public float ratio;
    [Tooltip("移動速度")] public float moveSpd;
    public float zenmaiRotationSpeed;
}

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [Tooltip("コントローラー番号")]
    public int controllerID = 0;

    Vector3 forwardDir;   // 移動方向の基準
    Vector3 rightDir;   // 移動方向の基準

    private Rigidbody rb;
    
    float moveSpd = 30f;  // 移動速度

    [SerializeField]
    TriggerSensor jumpSensor;   // ジャンプ可能判定用センサー
    
    private bool jumpable;  // 着地判定

    private bool movable = true; // 移動可能フラグ

    private bool gameStartMovable = false;
    
    Zenmai zenmai;  // ゼンマイ

    [Header("プランナー調整用")]


    private bool carryObjFg;
    [SerializeField]
    float carryMoveSpeed;

    [SerializeField]
    float jumpPower = 1500f;    //ジャンプ力

    [SerializeField]
    float gravity = -100f; // 重力
    
    // 移動方向
    Vector3 moveDir = Vector3.zero;

    ItemPocket myPocket;

    [SerializeField]
    private Animator playerAnim;

    private PhotonTransformViewClassic photonTransformView;
    [SerializeField]
    ZenmaiRotation zenmaiRotation;

    [SerializeField]
    BoxCollider actionCol;

    private Vector3 playerDir;

    [SerializeField]
    float stepInterval;

    [SerializeField]
    AudioClip footStepSE;
    [SerializeField]
    AudioClip jumpSE;

    //ジャンプしてから一定時間飛べなくするため
    private bool jumpbleTimeFg;

    private Coroutine nowCoroutine;

    // Start is called before the first frame update
    void Awake()
    {
        forwardDir = new Vector3(0f, 0f, 1f);
        rightDir = new Vector3(1f, 0f, 0f);
        rb = GetComponent<Rigidbody>();
        zenmai = GetComponent<Zenmai>();
        rb.sleepThreshold = -1;

        gameStartMovable = false;

        myPocket = GetComponent<ItemPocket>();
        photonTransformView = GetComponent<PhotonTransformViewClassic>();
        //自分以外は着地判定を取らない
        if(photonView.IsMine)
        {
            jumpSensor.enabled = true;
        }

        jumpbleTimeFg = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            
            moveSpd = carryMoveSpeed;

            moveDir = Vector3.zero;

            if (movable&& gameStartMovable)
            {
                if (Input.GetKey("w"))
                {
                    moveDir += forwardDir;
                }

                if (Input.GetKey("s"))
                {
                    moveDir -= forwardDir;
                }

                if (Input.GetKey("d"))
                {
                    moveDir += rightDir;
                    // キャラクターの大きさ。負数にすると反転される
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Abs(scale.x);  // 通常方向(スプライトと同じ右向き)
                    transform.localScale = scale;
                }

                if (Input.GetKey("a"))
                {
                    moveDir -= rightDir;

                    // キャラクターの大きさ。負数にすると反転される
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x); ;  // 通常方向(スプライトと同じ右向き)
                    transform.localScale = scale;
                }

                if (moveDir.magnitude == 0f)
                {
                    moveDir += rightDir * XInputManager.GetThumbStickLeftX(controllerID);
                    moveDir += forwardDir * XInputManager.GetThumbStickLeftY(controllerID);
                }
                
                //歩く音を止める
               
                moveDir.Normalize();

                if (jumpable == true)//着地しているとき
                {
                    if (Input.GetKeyDown("space") || XInputManager.GetButtonTrigger(controllerID, XButtonType.A))
                    {
                        if (myPocket.GetItem() == null&&!jumpbleTimeFg)
                        {
                            jumpable = false;
                            rb.AddForce(new Vector3(0, jumpPower, 0));
                            playerAnim.SetTrigger("Jumping");
                            SimpleAudioManager.PlayOneShot(jumpSE);
                            StartCoroutine(JampTimer());
                        }
                    }                    

                }
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Vector3 moveVec = moveDir * moveSpd;
            // 重力
            rb.AddForce(new Vector3(0, gravity, 0));
            rb.velocity = new Vector3(moveVec.x, rb.velocity.y, moveVec.z);           
            photonTransformView.SetSynchronizedValues(speed: rb.velocity, turnSpeed: 0);

            SetPlayerAnimation(moveVec);            
        }
    }

    private void SerWalkState()
    {
        playerAnim.SetBool("Walking", true);
        playerAnim.SetBool("Actioning", false);
        playerAnim.SetBool("Waiting", false);
        playerAnim.SetBool("Carry", false);

        //歩く音を鳴らす
        if (nowCoroutine == null)
        {
            nowCoroutine = StartCoroutine(FootStepSE());
        }
    }

    public void SetPlayerMovable(bool _fg)
    {
        movable = _fg;
    }

    private void SetPlayerAnimation(Vector3 _moveVec)
    {
        if (moveDir.magnitude > 0)
        {
            Vector3 sideColPos= new Vector3(-2.5f, -0.5f, 0f);
            Vector3 sideScale = new Vector3(3f, 1.5f, 1f);

            Vector3 forwardColPos= new Vector3(0f, -0.5f, 2.5f);
            Vector3 forwardScale = new Vector3(1f, 1.5f, 3f);
            zenmai.DecreaseZenmaiPower();

            if (_moveVec.x > 0f)
            {
                SerWalkState();
                playerAnim.SetBool("Back", false);
                playerAnim.SetBool("Side", true);
                playerAnim.SetBool("Forward", false);

                // キャラクターの大きさ。負数にすると反転される
                Vector3 scale = transform.localScale;
                scale.x = -Mathf.Abs(scale.x);  // 通常方向(スプライトと同じ右向き)
                transform.localScale = scale;

                zenmaiRotation.ChangeZenmaiPosition(true);

                actionCol.center = sideColPos;
                actionCol.size = sideScale;
                playerDir = Vector3.right;
            }
            else if (_moveVec.x < 0f)
            {
                SerWalkState();
                playerAnim.SetBool("Back", false);
                playerAnim.SetBool("Side", true);
                playerAnim.SetBool("Forward", false);

                // キャラクターの大きさ。負数にすると反転される
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x);  // 通常方向(スプライトと同じ右向き)
                transform.localScale = scale;

                zenmaiRotation.ChangeZenmaiPosition(true);

                actionCol.center = sideColPos;
                actionCol.size = sideScale;

                playerDir = -Vector3.right;
            }
            else if (_moveVec.z > 0f)
            {
                SerWalkState();
                playerAnim.SetBool("Back", true);
                playerAnim.SetBool("Side", false);
                playerAnim.SetBool("Forward", false);

                zenmaiRotation.ChangeZenmaiPosition(false);

                // キャラクターの大きさ。負数にすると反転される
                Vector3 scale = transform.localScale;
                scale.z = Mathf.Abs(scale.z);  // 通常方向(スプライトと同じ右向き)
                transform.localScale = scale;

                actionCol.center = forwardColPos;
                actionCol.size = forwardScale;

                playerDir = Vector3.forward;
            }
            else if (_moveVec.z < 0f)
            {
                SerWalkState();
                playerAnim.SetBool("Back", false);
                playerAnim.SetBool("Side", false);
                playerAnim.SetBool("Forward", true);

                zenmaiRotation.ChangeZenmaiPosition(false);
                // キャラクターの大きさ。負数にすると反転される
                Vector3 scale = transform.localScale;
                scale.z = -Mathf.Abs(scale.z);  // 通常方向(スプライトと同じ右向き)
                transform.localScale = scale;

                actionCol.center = forwardColPos;
                actionCol.size = forwardScale;

                playerDir = -Vector3.forward;
            }
        }
        //歩いていない
        else
        {
            if (nowCoroutine != null)
            {
                StopCoroutine(nowCoroutine);
                nowCoroutine = null;
            }

            playerAnim.SetBool("Walking", false);
            playerAnim.SetBool("Waiting", true);
        }
    }

    public void SetPlayerJumpable(bool _fg)
    {
        jumpable = _fg;
    }

    public void SetCarryObjFg(bool _fg)
    {
        carryObjFg = _fg;
    }

    public Vector3 GetPlayerDir()
    {
        return playerDir;
    }

    IEnumerator FootStepSE()
    {
        while (true)
        {
            SimpleAudioManager.PlayOneShot(footStepSE);
            yield return new WaitForSeconds(stepInterval);
        }
    }

    IEnumerator JampTimer()
    {
        jumpbleTimeFg = true;
        yield return new WaitForSeconds(0.1f);
        jumpbleTimeFg = false;

        yield  break;
    }

    public void StartPlayerMoving()
    {
        gameStartMovable = true;
    }
}

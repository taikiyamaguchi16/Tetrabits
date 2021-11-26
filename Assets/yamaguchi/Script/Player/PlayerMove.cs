using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public struct MoveSpeedInRatio
{
    [Tooltip("割合(0~1)")] public float ratio;
    [Tooltip("移動速度")] public float moveSpd;
}

public class PlayerMove : MonoBehaviourPunCallbacks
{
    [Tooltip("コントローラー番号")]
    public int controllerID = 0;

    [SerializeField,Tooltip("移動方向の基準")]
    Transform moveStandard = null;   // 移動方向の基準

    private Rigidbody rb;
    
    float moveSpd = 30f;  // 移動速度

    [SerializeField]
    TriggerSensor jumpSensor;   // ジャンプ可能判定用センサー
    
    private bool jumpable;  // 着地判定

    private bool movable = true; // 移動可能フラグ
    
    Zenmai zenmai;  // ゼンマイ

    [Header("プランナー調整用")]

    [SerializeField, Tooltip("ゼンマイパワーの割合とそれに応じた移動速度")]
    List<MoveSpeedInRatio> moveSpeedInRatios = new List<MoveSpeedInRatio>();

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

    // Start is called before the first frame update
    void Start()
    {
        moveStandard = Camera.main.transform;
        rb = GetComponent<Rigidbody>();
        zenmai = GetComponent<Zenmai>();

        // ソート
        for (int i = 0; i < moveSpeedInRatios.Count; i++)
        {
            for(int j = i + 1; j < moveSpeedInRatios.Count; j++)
            {
                if(moveSpeedInRatios[i].ratio < moveSpeedInRatios[j].ratio)
                {
                    MoveSpeedInRatio tmp = moveSpeedInRatios[i];
                    moveSpeedInRatios[i] = moveSpeedInRatios[j];
                    moveSpeedInRatios[j] = tmp;
                }
            }
        }

        myPocket = GetComponent<ItemPocket>();
        photonTransformView = GetComponent<PhotonTransformViewClassic>();
        //playerAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // ゼンマイパワーから速度決定
        float ratio = zenmai.zenmaiPower / zenmai.maxZenmaiPower;
        foreach (var speedInRatio in moveSpeedInRatios)
        {
            if(ratio <= speedInRatio.ratio)
                moveSpd = speedInRatio.moveSpd;
        }

        jumpable = jumpSensor.GetExistInTrigger();

        moveDir = Vector3.zero;
        if (photonView.IsMine)
        {
            if (movable)
            {
                if (Input.GetKey("w"))
                {
                    moveDir += moveStandard.transform.forward;
                    SerWalkState();
                    playerAnim.SetBool("Back", true);
                    playerAnim.SetBool("Side", false);
                    playerAnim.SetBool("Forward", false);
                }

                if (Input.GetKey("s"))
                {
                    moveDir -= moveStandard.transform.forward;
                    SerWalkState();
                    playerAnim.SetBool("Back", false);
                    playerAnim.SetBool("Side", false);
                    playerAnim.SetBool("Forward", true);
                }

                if (Input.GetKey("d"))
                {
                    moveDir += moveStandard.transform.right;
                    SerWalkState();
                    playerAnim.SetBool("Back", false);
                    playerAnim.SetBool("Side", true);
                    playerAnim.SetBool("Forward", false);

                    // キャラクターの大きさ。負数にすると反転される
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Abs(scale.x);  // 通常方向(スプライトと同じ右向き)
                    transform.localScale = scale;
                }

                if (Input.GetKey("a"))
                {
                    moveDir -= moveStandard.transform.right;
                    SerWalkState();
                    playerAnim.SetBool("Back", false);
                    playerAnim.SetBool("Side", true);
                    playerAnim.SetBool("Forward", false);

                    // キャラクターの大きさ。負数にすると反転される
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x); ;  // 通常方向(スプライトと同じ右向き)
                    transform.localScale = scale;
                }

                moveDir += moveStandard.transform.right * XInputManager.GetThumbStickLeftX(controllerID);
                moveDir += moveStandard.transform.forward * XInputManager.GetThumbStickLeftY(controllerID);

                moveDir.Normalize();

                if (jumpable == true)//着地しているとき
                {
                    if (Input.GetKeyDown("space") || XInputManager.GetButtonTrigger(controllerID, XButtonType.A))
                    {
                        if (myPocket.GetItem() == null)
                        {
                            jumpable = false;
                            rb.AddForce(new Vector3(0, jumpPower, 0));
                            playerAnim.SetTrigger("Jumping");
                        }
                    }

                }

                if (moveDir.x > 0f)
                {
                    SerWalkState();
                    playerAnim.SetBool("Back", false);
                    playerAnim.SetBool("Side", true);
                    playerAnim.SetBool("Forward", false);

                    // キャラクターの大きさ。負数にすると反転される
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Abs(scale.x);  // 通常方向(スプライトと同じ右向き)
                    transform.localScale = scale;
                }
                else if (moveDir.x < 0f)
                {
                    SerWalkState();
                    playerAnim.SetBool("Back", false);
                    playerAnim.SetBool("Side", true);
                    playerAnim.SetBool("Forward", false);

                    // キャラクターの大きさ。負数にすると反転される
                    Vector3 scale = transform.localScale;
                    scale.x = Mathf.Abs(scale.x);  // 通常方向(スプライトと同じ右向き)
                    transform.localScale = scale;
                }
                else if (moveDir.z > 0f)
                {
                    SerWalkState();
                    playerAnim.SetBool("Back", true);
                    playerAnim.SetBool("Side", false);
                    playerAnim.SetBool("Forward", false);
                }
                else if (moveDir.z < 0f)
                {
                    SerWalkState();
                    playerAnim.SetBool("Back", false);
                    playerAnim.SetBool("Side", false);
                    playerAnim.SetBool("Forward", true);
                }
                else
                {
                    playerAnim.SetBool("Walking", false);
                    playerAnim.SetBool("Waiting", true);
                }
            }
        }
    }
    
    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            Vector3 moveVec = moveDir * moveSpd;
            rb.velocity = new Vector3(moveVec.x, rb.velocity.y, moveVec.z);
            // 重力
            rb.AddForce(new Vector3(0, gravity, 0));
            photonTransformView.SetSynchronizedValues(speed: rb.velocity, turnSpeed: 0);

            if(moveDir.magnitude>0)
            {
                zenmai.DecreaseZenmaiPower();
            }
        }
    }

    private void SerWalkState()
    {
        playerAnim.SetBool("Walking", true);
        playerAnim.SetBool("Actioning", false);
        playerAnim.SetBool("Waiting", false);
        playerAnim.SetBool("Carry", false);
    }

    public void SetPlayerMovable(bool _fg)
    {
        movable = _fg;
    }
}

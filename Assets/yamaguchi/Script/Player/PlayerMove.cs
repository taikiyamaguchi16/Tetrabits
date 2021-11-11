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

    public bool movable = true; // 移動可能フラグ
    
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
                    playerAnim.SetBool("Walk", false);
                    playerAnim.SetBool("SideWalk", false);
                    playerAnim.SetBool("BackWalk", true);              
                }

                if (Input.GetKey("s"))
                {
                    moveDir -= moveStandard.transform.forward;
                    playerAnim.SetBool("Walk", true);
                    playerAnim.SetBool("SideWalk", false);
                    playerAnim.SetBool("BackWalk", false);
                }

                if (Input.GetKey("d"))
                {
                    moveDir += moveStandard.transform.right;
                    playerAnim.SetBool("Walk", false);
                    playerAnim.SetBool("SideWalk", true);
                    playerAnim.SetBool("BackWalk", false);
                }

                if (Input.GetKey("a"))
                {
                    moveDir -= moveStandard.transform.right;
                    playerAnim.SetBool("Walk", false);
                    playerAnim.SetBool("SideWalk", true);
                    playerAnim.SetBool("BackWalk", false);
                }

                moveDir += moveStandard.transform.right * XInputManager.GetThumbStickLeftX(controllerID);
                moveDir += moveStandard.transform.forward * XInputManager.GetThumbStickLeftY(controllerID);

                moveDir.Normalize();

                if (jumpable == true)//着地しているとき
                {
                    if (Input.GetKeyDown("space") || XInputManager.GetButtonTrigger(controllerID, XButtonType.A))
                    {
                        if (myPocket.GetItem()==null)
                        {
                            jumpable = false;
                            rb.AddForce(new Vector3(0, jumpPower, 0));
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
            rb.velocity = new Vector3(moveVec.x, rb.velocity.y, moveVec.z);

            // 重力
            rb.AddForce(new Vector3(0, gravity, 0));

            // キャラクターの大きさ。負数にすると反転される
            Vector3 scale = transform.localScale;
            if (rb.velocity.x > 1)      // 右方向に動いている
                scale.x = -1;  // 通常方向(スプライトと同じ右向き)
            else if (rb.velocity.x < -1) // 左方向に動いている
                scale.x = 1; // 反転
                              // 更新
            transform.localScale = scale;
        }
    }
}

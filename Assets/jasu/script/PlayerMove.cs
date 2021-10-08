using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[System.Serializable]
public struct MoveSpeedInRatio
{
    [Tooltip("割合(0~1)")]　    public float ratio;
    [Tooltip("移動速度")]  public float moveSpd;
}

public class PlayerMove : MonoBehaviourPunCallbacks
{
    public int playerNum = 0;

    [SerializeField]
    Transform moveStandard = null;   // 移動方向の基準

    private Rigidbody rb;

    private bool isGround;  // 着地判定
    
    float moveSpd = 0.03f;  // 移動速度

    bool inputted = false;

    public bool movable = true; // 移動可能フラグ
    
    Zenmai zenmai;  // ゼンマイ

    [Header("プランナー調整用")]

    [SerializeField, Tooltip("ゼンマイパワーの割合とそれに応じた移動速度")]
    List<MoveSpeedInRatio> moveSpeedInRatios = new List<MoveSpeedInRatio>();

    [SerializeField]
    float jumpPower = 200f;    //ジャンプ力
    
    // 移動方向
    Vector3 moveDir = Vector3.zero;

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
    }

    // Update is called once per frame
    void Update()
    {
        float ratio = zenmai.zenmaiPower / zenmai.maxZenmaiPower;
        foreach (var speedInRatio in moveSpeedInRatios)
        {
            if(ratio <= speedInRatio.ratio)
                moveSpd = speedInRatio.moveSpd;
        }
        
        moveDir = Vector3.zero;
        //if (photonView.IsMine)
       // {
            if (movable)
            {
                if (Input.GetKey("w"))
                {
                    moveDir += moveStandard.transform.forward;
                }

                if (Input.GetKey("s"))
                {
                    moveDir -= moveStandard.transform.forward;
                }

                if (Input.GetKey("d"))
                {
                    moveDir += moveStandard.transform.right;
                }

                if (Input.GetKey("a"))
                {
                    moveDir -= moveStandard.transform.right;
                }

                moveDir += moveStandard.transform.right * XInputManager.GetThumbStickLeftX(playerNum);
                moveDir += moveStandard.transform.forward * XInputManager.GetThumbStickLeftY(playerNum);

                moveDir.Normalize();

                if (isGround == true)//着地しているとき
                {
                    if (Input.GetKeyDown("space") || XInputManager.GetButtonTrigger(playerNum, XButtonType.A))
                    {
                        isGround = false;
                        rb.AddForce(new Vector3(0, jumpPower, 0));
                    }
                }
            //}
        }
    }

    private void FixedUpdate()
    {
        //rb.AddForce(moveDir * moveSpd, ForceMode.Force);
        Vector3 moveVec = moveDir * moveSpd;
        rb.velocity = new Vector3(moveVec.x, rb.velocity.y, moveVec.z);

        // 重力
        //rb.AddForce(new Vector3(0, -3000, 0));
    }

    void OnCollisionEnter(Collision other) //地面に触れた時の処理
    {
        if (other.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }
}

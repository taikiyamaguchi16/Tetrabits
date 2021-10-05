using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MoveSpeedInRatio
{
    [Tooltip("割合(0~1)")]　    public float ratio;
    [Tooltip("移動速度")]  public float moveSpd;
}

public class PlayerMove : MonoBehaviour
{
    public int playerNum = 0;

    [SerializeField]
    Camera camera = null;   // 移動方向用カメラ

    private Rigidbody rb;

    private bool isGround;   // 着地判定
    
    float moveSpd = 0.03f;  // 移動速度

    public bool movable = true; // 移動可能フラグ
    
    Zenmai zenmai;  // ゼンマイ

    [Header("プランナー調整用")]

    [SerializeField, Tooltip("ゼンマイパワーの割合とそれに応じた移動速度")]
    List<MoveSpeedInRatio> moveSpeedInRatios = new List<MoveSpeedInRatio>();

    [SerializeField]
    float jumpPower = 200f;    //ジャンプ力

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
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

        if (movable)
        {
            if (Input.GetKey("w"))
            {
                rb.velocity += camera.transform.forward * moveSpd;
            }

            if (Input.GetKey("s"))
            {
                rb.velocity -= camera.transform.forward * moveSpd;
            }

            if (Input.GetKey("d"))
            {
                rb.velocity += camera.transform.right * moveSpd;
            }

            if (Input.GetKey("a"))
            {
                rb.velocity -= camera.transform.right * moveSpd;
            }

            rb.velocity += camera.transform.right * moveSpd * XInputManager.GetThumbStickLeftX(playerNum);
            rb.velocity += camera.transform.forward * moveSpd * XInputManager.GetThumbStickLeftY(playerNum);

            if (isGround == true)//着地しているとき
            {
                if (Input.GetKeyDown("space") || XInputManager.GetButtonTrigger(playerNum, XButtonType.A))
                {
                    isGround = false;
                    rb.AddForce(new Vector3(0, jumpPower, 0));
                }
            }
        }
    }

    void OnCollisionEnter(Collision other) //地面に触れた時の処理
    {
        if (other.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    Camera camera = null;   // 移動方向用カメラ

    private Rigidbody rb;

    [SerializeField]
    float upForce = 200f;    //ジャンプ力

    private bool isGround;   // 着地判定

    [SerializeField]
    float moveSpd = 0.03f;

    [SerializeField]
    float maxSpd = 0.03f;

    [SerializeField]
    float minSpd = 0.01f;

    public bool movable = true;
    
    Zenmai zenmai;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
        zenmai = GetComponent<Zenmai>();
        moveSpd = maxSpd;
    }

    // Update is called once per frame
    void Update()
    {
        moveSpd = zenmai.zenmaiPower / zenmai.maxZenmaiPower * maxSpd;
        if (moveSpd < minSpd)
            moveSpd = minSpd;


        if (movable)
        {
            if (Input.GetKey("w"))
            {
                transform.position += camera.transform.forward * moveSpd;
            }

            if (Input.GetKey("s"))
            {
                transform.position -= camera.transform.forward * moveSpd;
            }

            if (Input.GetKey("d"))
            {
                transform.position += camera.transform.right * moveSpd;
            }

            if (Input.GetKey("a"))
            {
                transform.position -= camera.transform.right * moveSpd;
            }

            if (isGround == true)//着地しているとき
            {
                if (Input.GetKeyDown("space"))
                {
                    isGround = false;
                    rb.AddForce(new Vector3(0, upForce, 0));
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

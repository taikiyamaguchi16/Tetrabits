using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField]
    Camera camera = null;

    private Rigidbody rb;

    [SerializeField]
    float upForce = 200f;    //ジャンプ力

    private bool isGround;          // 着地判定

    [SerializeField]
    float moveSpd = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        camera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
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

    void OnCollisionEnter(Collision other) //地面に触れた時の処理
    {
        if (other.gameObject.tag == "Ground")
        {
            isGround = true;
        }
    }
}

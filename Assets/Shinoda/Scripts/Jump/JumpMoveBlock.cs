using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpMoveBlock : MonoBehaviour
{
    //変数定義
    Rigidbody2D rb;
    SurfaceEffector2D surfaceEffector;
    Vector2 DefaultPos;
    Vector2 PrevPos;

    [SerializeField] float moveX;
    [SerializeField] float moveY;

    [SerializeField] bool right = true;
    [SerializeField] bool up = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        DefaultPos = transform.position;
        surfaceEffector = GetComponent<SurfaceEffector2D>();
    }

    void FixedUpdate()
    {
        PrevPos = rb.position;

        Vector2 pos = Vector2.zero;
        if (right && up)
        {
            if (moveX != 0 && moveY != 0) pos = new Vector2(DefaultPos.x + Mathf.PingPong(Time.time, moveX), DefaultPos.y + Mathf.PingPong(Time.time, moveY));
            else if (moveX == 0 && moveY != 0) pos = new Vector2(DefaultPos.x, DefaultPos.y + Mathf.PingPong(Time.time, moveY));
            else if (moveX != 0 && moveY == 0) pos = new Vector2(DefaultPos.x + Mathf.PingPong(Time.time, moveX), DefaultPos.y);
            else pos = new Vector2(DefaultPos.x, DefaultPos.y);
        }
        else if (right && !up)
        {
            if (moveX != 0 && moveY != 0) pos = new Vector2(DefaultPos.x + Mathf.PingPong(Time.time, moveX), DefaultPos.y - Mathf.PingPong(Time.time, moveY));
            else if (moveX == 0 && moveY != 0) pos = new Vector2(DefaultPos.x, DefaultPos.y - Mathf.PingPong(Time.time, moveY));
            else if (moveX != 0 && moveY == 0) pos = new Vector2(DefaultPos.x + Mathf.PingPong(Time.time, moveX), DefaultPos.y);
            else pos = new Vector2(DefaultPos.x, DefaultPos.y);
        }
        else if (!right && up)
        {
            if (moveX != 0 && moveY != 0) pos = new Vector2(DefaultPos.x - Mathf.PingPong(Time.time, moveX), DefaultPos.y + Mathf.PingPong(Time.time, moveY));
            else if (moveX == 0 && moveY != 0) pos = new Vector2(DefaultPos.x, DefaultPos.y + Mathf.PingPong(Time.time, moveY));
            else if (moveX != 0 && moveY == 0) pos = new Vector2(DefaultPos.x - Mathf.PingPong(Time.time, moveX), DefaultPos.y);
            else pos = new Vector2(DefaultPos.x, DefaultPos.y);
        }
        else if (!right && !up)
        {
            if (moveX != 0 && moveY != 0) pos = new Vector2(DefaultPos.x - Mathf.PingPong(Time.time, moveX), DefaultPos.y - Mathf.PingPong(Time.time, moveY));
            else if (moveX == 0 && moveY != 0) pos = new Vector2(DefaultPos.x, DefaultPos.y - Mathf.PingPong(Time.time, moveY));
            else if (moveX != 0 && moveY == 0) pos = new Vector2(DefaultPos.x - Mathf.PingPong(Time.time, moveX), DefaultPos.y);
            else pos = new Vector2(DefaultPos.x, DefaultPos.y);
        }
        rb.MovePosition(pos);

        Vector2 velocity = (pos - PrevPos) / Time.deltaTime;

        surfaceEffector.speed = velocity.magnitude;
    }
}
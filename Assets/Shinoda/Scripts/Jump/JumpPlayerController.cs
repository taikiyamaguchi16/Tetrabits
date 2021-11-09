using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    float moveSpeed;
    [SerializeField] float jumpForce = 10f;

    float gravityScale = 3f;
    bool isParasol = false;
    bool isJump = false;

    enum MoveType
    {
        Stop,
        Right,
        Left,
    }
    MoveType move = MoveType.Stop;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();

        if (padVec.x > 0)
        {
            move = MoveType.Right;
        }
        else if (padVec.x < 0)
        {
            move = MoveType.Left;
        }
        else
        {
            move = MoveType.Stop;
        }

        if(TetraInput.sTetraButton.GetTrigger())
        {
            Jump();
            isJump = true;
        }

        if (TetraInput.sTetraLever.GetPoweredOn()) rb.gravityScale = 0.5f;
        else rb.gravityScale = 1;
    }

    private void FixedUpdate()
    {
        if (move == MoveType.Stop)
        {
            moveSpeed = 0;
        }
        else if (move == MoveType.Right)
        {
            moveSpeed = 3;
        }
        else if (move == MoveType.Left)
        {
            moveSpeed = -3;
        }
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isJump = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerController : MonoBehaviour
{
    Rigidbody2D rb;

    [SerializeField] float jumpForce = 10f;

    float gravityScale = 3f;
    bool isParasol = false;
    bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();
        if (TetraInput.sTetraButton.GetTrigger())
        {
            Jump(padVec);
            isJump = true;
        }

        if (isJump && TetraInput.sTetraLever.GetPoweredOn())
        {
            rb.gravityScale = 0.5f;
            isParasol = true;
        }
        else
        {
            rb.gravityScale = 1;
            isParasol = false;
        }
    }

    private void FixedUpdate()
    {

    }

    void Jump(Vector2 _jumpDirection)
    {
        rb.AddForce(_jumpDirection * jumpForce, ForceMode2D.Impulse);
    }
}
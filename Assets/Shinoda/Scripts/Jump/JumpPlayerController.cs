using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    bool isParasol = false;
    bool isJump = false;
    public GameObject lastFlag;
    Vector2 padVec;
    Animator animator;
    float beforeVelocityY;
    bool beforeParasol;

    [Header("Player")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float moveScale = 3f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float parasolGravity = .5f;
    [SerializeField] float speedLimit = 5f;
    [SerializeField] bool upOnly;

    [Header("Arrow")]
    Vector3 originScale;
    [SerializeField] GameObject arrow;
    [SerializeField] float arrowScaleRatio = 1f;

    // Start is called before the first frame update
    void Start()
    {
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        originScale = arrow.transform.localScale;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        padVec = TetraInput.sTetraPad.GetVector();
        ArrowControll(padVec);
        if (TetraInput.sTetraButton.GetTrigger() && !isJump) Jump(padVec);

        if (TetraInput.sTetraLever.GetPoweredOn()) isParasol = true;
        else isParasol = false;

        // 落下開始時にparasol状態なら傘を開く
        if (beforeVelocityY >= 0 && rb.velocity.y < 0)
        {
            Debug.Log("落下開始");
            animator.SetBool("stand", false);
            animator.SetBool("jump", false);
            if (isParasol) animator.SetBool("open", true);
        }
        if (rb.velocity.y < 0)
        {
            if (beforeParasol && !isParasol)
            {
                animator.SetBool("open", false);
                animator.SetBool("close", true);
            }
            else if (!beforeParasol && isParasol)
            {
                animator.SetBool("close", false);
                animator.SetBool("open", true);
            }
        }

        //isParasol中の落下速度軽減
        if (isParasol && rb.velocity.y < 0) rb.gravityScale = parasolGravity;
        else rb.gravityScale = gravityScale;

        beforeVelocityY = rb.velocity.y;
        beforeParasol = isParasol;
    }

    void FixedUpdate()
    {
        if (isJump)
        {
            if (padVec.x > 0 && rb.velocity.x < speedLimit)
            {
                rb.AddForce(transform.right * padVec.x * moveScale);
            }
            else if (padVec.x < 0 && rb.velocity.x > -speedLimit)
            {
                rb.AddForce(transform.right * padVec.x * moveScale);
            }
        }
    }

    void Jump(Vector2 _jumpDirection)
    {
        if (upOnly)
        {
            if (padVec.y > 0) rb.AddForce(_jumpDirection * jumpForce, ForceMode2D.Impulse);
        }
        else rb.AddForce(_jumpDirection * jumpForce, ForceMode2D.Impulse);
    }

    void ArrowControll(Vector2 _dir)
    {
        arrow.transform.localScale = originScale * (_dir.magnitude * arrowScaleRatio);
        arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
    }

    public void JumpOn()
    {
        isJump = true;
        animator.SetBool("stand", false);
        animator.SetBool("jump", true);
    }

    public void JumpOff()
    {
        isJump = false;
        animator.SetBool("open", false);
        animator.SetBool("jump", false);
        animator.SetBool("close", false);
        animator.SetBool("stand", true);
    }
}
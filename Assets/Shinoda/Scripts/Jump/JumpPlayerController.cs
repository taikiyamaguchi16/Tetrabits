﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpPlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer myRenderer;
    bool isParasol = false;
    bool isJump = false;
    public bool GetJump() { return isJump; }
    public GameObject lastFlag;
    Vector2 padVec;
    Animator animator;
    float beforeVelocityY;
    bool beforeParasol;
    bool playAnim = false;

    [Header("Player")]
    [SerializeField] float jumpForce = 10f;
    [SerializeField] float moveScale = 3f;
    [SerializeField] float gravityScale = 1f;
    [SerializeField] float parasolGravity = .5f;
    [SerializeField] float speedLimit = 5f;
    [SerializeField] bool upOnly;

    [SerializeField] AudioClip BGM;
    [SerializeField] AudioClip jumpSE;
    [SerializeField] AudioClip hitSE;

    [Header("Arrow")]
    Vector3 originScale;
    [SerializeField] GameObject arrow;
    [SerializeField] float arrowScaleRatio = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SimpleAudioManager.PlayBGMCrossFade(BGM, 1.0f);
        GameInGameUtil.StartGameInGameTimer("jump");
        rb = this.gameObject.GetComponent<Rigidbody2D>();
        originScale = arrow.transform.localScale;
        animator = GetComponent<Animator>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        padVec = TetraInput.sTetraPad.GetVector();
        ArrowControll(padVec);
        if (TetraInput.sTetraButton.GetTrigger() && !isJump) Jump(padVec);

        if (TetraInput.sTetraLever.GetPoweredOn()) isParasol = true;
        else isParasol = false;

        if (beforeParasol && !isParasol)
        {
            AnimAllOff();
            animator.SetBool("standClose", true);
        }
        else if (!beforeParasol && isParasol)
        {
            AnimAllOff();
            animator.SetBool("standOpen", true);
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
        SimpleAudioManager.PlayOneShot(jumpSE);
        if (padVec.x == 0 && padVec.y == 0) rb.AddForce(transform.up.normalized * 2f, ForceMode2D.Impulse);
        else if (padVec.y > 0) rb.AddForce(_jumpDirection * jumpForce, ForceMode2D.Impulse);
    }

    void ArrowControll(Vector2 _dir)
    {
        arrow.transform.localScale = originScale * (_dir.magnitude * arrowScaleRatio);
        arrow.transform.rotation = Quaternion.FromToRotation(Vector3.up, _dir);
    }

    public void JumpOn()
    {
        isJump = true;
    }

    public void JumpOff()
    {
        isJump = false;
        AnimAllOff();
        if (isParasol) animator.SetBool("standParasol", true);
        else animator.SetBool("standNormal", true);
    }

    public void DamageAnimation()
    {
        myRenderer.DOColor(Color.red, 0.5f).SetEase(Ease.OutFlash, 4, 0.5f);
    }

    void AnimAllOff()
    {
        animator.SetBool("standOpen", false);
        animator.SetBool("standClose", false);
        animator.SetBool("standParasol", false);
        animator.SetBool("standNormal", false);
    }
}
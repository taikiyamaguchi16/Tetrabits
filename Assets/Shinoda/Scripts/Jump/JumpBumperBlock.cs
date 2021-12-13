using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JumpBumperBlock : MonoBehaviour
{
    GameObject player;
    Rigidbody2D playerRb;

    Vector3 originPos;

    [SerializeField] float moveX;
    [SerializeField] float moveY;
    [SerializeField] float blockSize = 1f;
    Vector3 targetPos;

    [SerializeField] float moveTime = 1f;

    [SerializeField] float bouncePower = 3f;
    Vector2 bounceDir;

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("JumpMan");
        playerRb = player.GetComponent<Rigidbody2D>();

        originPos = transform.position;
        targetPos = new Vector3(originPos.x + (moveX * blockSize), originPos.y + (moveY * blockSize), originPos.z);
        // 移動床設定
        this.transform.DOMove(targetPos, moveTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bounceDir = player.transform.position - this.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            playerRb.AddForce(bounceDir.normalized * bouncePower, ForceMode2D.Impulse);
            animator.SetTrigger("bumper");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player)
        {
            playerRb.AddForce(bounceDir.normalized * bouncePower, ForceMode2D.Impulse);
            animator.SetTrigger("bumper");
        }
    }
}
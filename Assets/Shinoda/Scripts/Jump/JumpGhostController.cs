using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpGhostController : MonoBehaviour
{
    GameObject player;
    GameObject playerFoot;
    Rigidbody2D playerRb;
    JumpPlayerController playerControllerComponent;
    bool beforeJump;

    Rigidbody2D myRigidbody;
    Vector3 targetPos;
    Vector3 targetDir;
    float timeCount;
    float distance;

    [SerializeField, Tooltip("ダメージ量")] string damage = "small";
    [SerializeField] float bouncePower = 5f;
    [SerializeField] float attackTime = 5f;
    [SerializeField] float ghostSpeed = 1.5f;
    Vector2 bounceDir;

    enum JumpGhostState
    {
        Stop,
        Move
    };
    JumpGhostState ghostState = JumpGhostState.Stop;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("JumpMan");
        playerFoot = player.transform.Find("Foot").gameObject;
        playerRb = player.GetComponent<Rigidbody2D>();
        playerControllerComponent = player.GetComponent<JumpPlayerController>();
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // ジャンプしてない時間の継続確認
        if (!playerControllerComponent.GetJump() && !beforeJump)
        {
            timeCount += Time.deltaTime;
        }
        else timeCount = 0;

        distance = (targetPos - this.transform.position).magnitude;

        if (timeCount > attackTime)
        {
            ghostState = JumpGhostState.Move;
            targetPos = player.transform.position;
            targetDir = (targetPos - this.transform.position).normalized;
        }
        else if (distance < .5f)
        {
            ghostState = JumpGhostState.Stop;
        }

        bounceDir = player.transform.position - this.transform.position;
        beforeJump = playerControllerComponent.GetJump();
    }

    private void FixedUpdate()
    {
        if (ghostState == JumpGhostState.Move)
        {
            myRigidbody.AddForce(targetDir * ghostSpeed);
        }
        else if (ghostState == JumpGhostState.Stop)
        {
            myRigidbody.velocity = new Vector2(0, 0);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == player && ghostState == JumpGhostState.Move)
        {
            playerRb.AddForce(bounceDir.normalized * bouncePower, ForceMode2D.Impulse);
            if (PhotonNetwork.IsMasterClient) MonitorManager.DealDamageToMonitor(damage);
            ghostState = JumpGhostState.Stop;
            timeCount = 0;

        }
    }
}
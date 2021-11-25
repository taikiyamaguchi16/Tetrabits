using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBumperBlock : MonoBehaviour
{
    GameObject player;
    Rigidbody2D playerRb;

    [SerializeField] float bouncePower = 3f;
    Vector2 bounceDir;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("JumpMan");
        playerRb = player.GetComponent<Rigidbody2D>();
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
        }
    }
}
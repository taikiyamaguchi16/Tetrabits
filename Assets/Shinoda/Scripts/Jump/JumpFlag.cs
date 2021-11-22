using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpFlag : MonoBehaviour
{
    GameObject player;
    JumpPlayerController playerControllerComponent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerControllerComponent = player.GetComponent<JumpPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject==player)
        {
            playerControllerComponent.lastFlag = this.gameObject;
        }
    }
}

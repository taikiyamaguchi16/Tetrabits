using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpFlag : MonoBehaviour
{
    GameObject player;
    JumpPlayerController playerControllerComponent;

    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;
    SpriteRenderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("JumpMan");
        playerControllerComponent = player.GetComponent<JumpPlayerController>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControllerComponent.lastFlag == this.gameObject) myRenderer.sprite = onSprite;
        else myRenderer.sprite = offSprite;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject==player)
        {
            playerControllerComponent.lastFlag = this.gameObject;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPlayerFoot : MonoBehaviour
{
    [SerializeField] JumpPlayerController playerControllerComponent;

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag != "Player") playerControllerComponent.isJump = false;
    //}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") playerControllerComponent.JumpOn();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player") playerControllerComponent.JumpOff();
    }
}
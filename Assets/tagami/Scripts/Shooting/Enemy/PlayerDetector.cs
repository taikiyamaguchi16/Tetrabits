using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public bool foundPlayer { private set; get; }

    public Vector3 foundPlayerPosition { private set; get; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (foundPlayer) return;

        //プレイヤー見つけたら突進
        if (collision.CompareTag("Player"))
        {
            foundPlayer = true;
            foundPlayerPosition = collision.transform.position;
        }
    }
}

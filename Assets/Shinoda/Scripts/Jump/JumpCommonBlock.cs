using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCommonBlock : MonoBehaviour
{
    [SerializeField] bool isStop;

    GameObject player;
    GameObject playerFoot;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("JumpMan");
        playerFoot = player.transform.Find("Foot").gameObject;
        //playerFoot = GameObject.Find("Foot");
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            if (isStop) player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
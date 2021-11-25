using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpDamageBlock : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージ量")] string damage = "small";
    [SerializeField, Tooltip("何秒でダメージが発生するか")] float time;

    float timeCount;
    GameObject player;
    GameObject playerFoot;

    // Start is called before the first frame update
    void Start()
    {
        timeCount = time;
        player = GameObject.Find("JumpMan");
        playerFoot = player.transform.Find("Foot").gameObject;
        //playerFoot = GameObject.Find("Foot");
    }

    // Update is called once per frame
    void Update()
    {
        if(timeCount<0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor(damage);
            }
            timeCount = time;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            timeCount -= Time.deltaTime;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            timeCount = time;
        }
    }
}

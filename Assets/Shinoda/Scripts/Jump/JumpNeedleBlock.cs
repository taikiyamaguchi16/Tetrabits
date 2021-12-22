using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpNeedleBlock : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージ量")] string damage = "small";
    [SerializeField, Tooltip("何秒でダメージが発生するか")] float time;

    float timeCount;
    GameObject player;
    GameObject playerFoot;
    JumpPlayerController playerControllerComponent;
    bool playerOn = false;

    // Start is called before the first frame update
    void Start()
    {
        timeCount = time;
        player = GameObject.Find("JumpMan");
        playerFoot = player.transform.Find("Foot").gameObject;
        playerControllerComponent = player.GetComponent<JumpPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerOn) timeCount -= Time.deltaTime;

        if (timeCount < 0)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor(damage);
            }
            playerControllerComponent.DamageAnimation();
            timeCount = time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && PhotonNetwork.IsMasterClient)
        {
            MonitorManager.DealDamageToMonitor(damage);
            playerControllerComponent.DamageAnimation();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            playerOn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == playerFoot)
        {
            timeCount = time;
            playerOn = false;
        }
    }
}
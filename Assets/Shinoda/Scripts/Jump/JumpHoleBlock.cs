using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class JumpHoleBlock : MonoBehaviour
{
    [SerializeField, Tooltip("ダメージ量")] string damage = "small";

    GameObject player;
    GameObject playerFoot;
    JumpPlayerController playerControllerComponent;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerFoot = player.transform.Find("Foot").gameObject;
        playerControllerComponent = player.GetComponent<JumpPlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject==playerFoot)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                MonitorManager.DealDamageToMonitor(damage);
            }
            player.transform.position = playerControllerComponent.lastFlag.transform.position;
        }
    }
}

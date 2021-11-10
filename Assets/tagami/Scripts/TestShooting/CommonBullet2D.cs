using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CommonBullet2D : MonoBehaviourPunCallbacks
{
    [SerializeField] string targetTag = "Enemy";

    [SerializeField] bool destroyGameClear; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameInGameUtil.IsMasterClient()) return;

        if (collision.gameObject.CompareTag(targetTag))
        {
            if(destroyGameClear)
            {
                GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                Destroy(collision.gameObject);
            }

            //Destroy(collision.gameObject);

            //自身弾の消去
            Destroy(gameObject);
            MonitorManager.DealDamageToMonitor("small");
        }
    }

}

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
        if (collision.gameObject.CompareTag(targetTag))
        {
            if(destroyGameClear)
            {
                GameInGameManager.sCurrentGameInGameManager.isGameEnd = true;
                GameInGameUtil.StopGameInGameTimer("shooting");
                Destroy(collision.gameObject);
            }

            //Destroy(collision.gameObject);

            if (GameInGameUtil.IsMasterClient())
            {//マスターのみが行う処理
                MonitorManager.DealDamageToMonitor("medium");
            }

            //自身弾の消去
            Destroy(gameObject);           
        }
    }

}

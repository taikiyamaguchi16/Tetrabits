using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGameCanvas : MonoBehaviour
{
    [SerializeField] Text lifeText;
    [SerializeField] Text bombText;


    // Update is called once per frame
    void Update()
    {
        lifeText.text = "残りライフ：" + ShootingGameManager.sLife;

        bombText.text = "残りボム数：" + ShootingGameManager.sBombNum;
    }
}

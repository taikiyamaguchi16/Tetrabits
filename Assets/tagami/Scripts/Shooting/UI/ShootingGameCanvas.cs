using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGameCanvas : MonoBehaviour
{
    [SerializeField] Text lifeText;

    // Update is called once per frame
    void Update()
    {
        lifeText.text = "残りライフ：" + ShootingGameManager.sShootingGameManager.life;
    }
}

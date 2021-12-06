using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingGameCanvas : MonoBehaviour
{
    [SerializeField] Text lifeText;
    [SerializeField] Generic.UIEasing lifeImageEasing;
    [SerializeField] int numLessRedAlert = 3;
    [SerializeField] Text bombText;


    // Update is called once per frame
    void Update()
    {

        lifeText.text = "×" + ShootingGameManager.sShootingGameManager.life;
        if (ShootingGameManager.sShootingGameManager.life < numLessRedAlert)
        {//アラート
            lifeText.color = Color.red;
            lifeImageEasing.enabled = true;
        }
        else
        {
            lifeText.color = Color.white;
            lifeImageEasing.enabled = false;
        }

        bombText.text = "×" + ShootingGameManager.sShootingGameManager.bomb;
        if(ShootingGameManager.sShootingGameManager.bomb<=0)
        {
            bombText.color = Color.red;
        }
        else
        {
            bombText.color = Color.white;
        }

    }
}

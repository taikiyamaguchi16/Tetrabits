using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shooting
{
    public class ShootingEnabledStageClear : MonoBehaviour,IActivate
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


        public void OnActivated()
        {
            Debug.Log("ShootingStageクリアー！");
            ShootingGameManager.sShootingGameManager.StageClear();
        }
    }
}
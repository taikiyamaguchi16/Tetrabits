using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSkiper : MonoBehaviour
{
    [Header("Skip After Scene")]
    [SerializeField] Trisibo.SceneField skipAfterScene;

    [Header("Skip Gauge")]
    [SerializeField] Slider skipGaugeSlider;
    [SerializeField] float skipableSeconds = 5.0f;
    float skipTimer;

    bool skiped;

    // Start is called before the first frame update
    void Start()
    {
        skipGaugeSlider.maxValue = skipableSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            skipTimer += Time.deltaTime;
            if (skipTimer >= skipableSeconds)
            {
                skipTimer = skipableSeconds;
                if (!skiped && Photon.Pun.PhotonNetwork.IsMasterClient)
                {
                    skiped = true;
                    GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(skipAfterScene.BuildIndex));
                }
            }
        }
        else
        {
            skipTimer -= Time.deltaTime;
            if (skipTimer < 0)
            {
                skipTimer = 0;
            }
        }

        //Gauge更新
        skipGaugeSlider.value = skipTimer;
    }
}

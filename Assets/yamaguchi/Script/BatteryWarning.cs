﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class BatteryWarning : MonoBehaviourPunCallbacks
{
    [SerializeField]
    float interval;

    [SerializeField]
    float blinkingTime;

    [SerializeField]
    BatteryHolder batteryHolder;

    [SerializeField]
    Image blinkImage;

    [SerializeField]
    Image nothingImage;

    [SerializeField]
    float startWarningLevel;

    [SerializeField]
    GameObject controllUiCanvas;

    private bool blinkingNow;

    private Coroutine nowCoroutine;

    private void Awake()
    {
        blinkingNow = false;
    }

    void Update()
    {
        //残量が0の場合のUI表示
        if(batteryHolder.GetBatterylevel()<=0f)
        {
            nothingImage.enabled = true;
            if (controllUiCanvas.activeSelf)
                nothingImage.enabled = false;

            if (nowCoroutine != null)
            {
                StopCoroutine(nowCoroutine);
                blinkImage.enabled = false;
            }
        }
        else
        {
            nothingImage.enabled = false;

            if (PhotonNetwork.IsMasterClient)
            {
                if (batteryHolder.GetBatterylevel() < startWarningLevel)
                {
                    if (!blinkingNow)
                    {                       
                        photonView.RPC(nameof(StartBlinkWarning), RpcTarget.All);
                    }
                }               
            }
            if(batteryHolder.GetBatterylevel() > startWarningLevel)
            {
                
                //コルーチン走っている場合
                if (nowCoroutine != null)
                {
                    StopCoroutine(nowCoroutine);
                    blinkImage.enabled = false;
                }
            }
        }

        
    }

    [PunRPC]
    IEnumerator StartBlinkWarning()
    {
        blinkingNow = true;
        nothingImage.enabled = false;

        nowCoroutine = StartCoroutine(BlinkWarning());
        yield return new WaitForSeconds(blinkingTime);
        StopCoroutine(nowCoroutine);

        blinkImage.enabled = false;
        blinkingNow = false;
        yield break;
    }
    IEnumerator BlinkWarning()
    {
        while (true)
        {
            blinkImage.enabled = !blinkImage.enabled;
            //アクションのUIが出ている場合点滅UIを非表示
            if (controllUiCanvas.activeSelf)
                blinkImage.enabled = false;
            yield return new WaitForSeconds(interval);
        }
    }

    [PunRPC]
    private void StoPWarningBlink()
    { }
}

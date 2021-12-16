using System.Collections;
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
    Image[] blinkImage = new Image[2];

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
                blinkImage[0].enabled = false;
                blinkImage[1].enabled = false;
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
                if (nowCoroutine != null)
                {
                    StopCoroutine(nowCoroutine);
                    blinkImage[0].enabled = false;
                    blinkImage[1].enabled = false;
                }
            }
        }

        
    }

    [PunRPC]
    IEnumerator StartBlinkWarning()
    {
        blinkingNow = true;

        nowCoroutine = StartCoroutine(BlinkWarning());
        yield return new WaitForSeconds(blinkingTime);
        StopCoroutine(nowCoroutine);

        blinkImage[0].enabled = false;
        blinkImage[1].enabled = false;
        blinkingNow = false;
        yield break;
    }
    IEnumerator BlinkWarning()
    {
        while (true)
        {
            blinkImage[0].enabled = !blinkImage[0].enabled;
            blinkImage[1].enabled = !blinkImage[1].enabled;
            yield return new WaitForSeconds(interval);
        }
    }

    [PunRPC]
    private void StoPWarningBlink()
    { }
}

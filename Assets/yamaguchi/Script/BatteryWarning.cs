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
    AudioClip warningSound;

    private  Coroutine soundCoroutine;
    private static int playSoundNum = 0;

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
        playSoundNum = 0;
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
                playSoundNum--;
                if (playSoundNum == 0 && soundCoroutine != null)
                {
                    StopCoroutine(soundCoroutine);
                }
                if (nowCoroutine != null)
                {
                    StopCoroutine(nowCoroutine);
                }
                blinkImage.enabled = false;
                blinkingNow = false;
                nowCoroutine = null;
            }
        }
        else
        {
            nothingImage.enabled = false;

  
            if (batteryHolder.GetBatterylevel() <= startWarningLevel)
            {
                if (!blinkingNow)
                {
                    blinkingNow = true;
                    nothingImage.enabled = false;
                    nowCoroutine = StartCoroutine(BlinkWarning());

                    playSoundNum++;
                }
            }               

            if(batteryHolder.GetBatterylevel() > startWarningLevel)
            {               
                //コルーチン走っている場合
                if (nowCoroutine != null)
                {
                    playSoundNum--;
                    if(playSoundNum==0&& soundCoroutine!=null)
                    {
                        StopCoroutine(soundCoroutine);
                    }
                    if (nowCoroutine != null)
                    {
                        StopCoroutine(nowCoroutine);
                    }
                    blinkImage.enabled = false;
                    blinkingNow = false;
                    nowCoroutine = null;
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
 
            yield break;
    }
    IEnumerator BlinkWarning()
    {
        if (playSoundNum == 0)
        {
            soundCoroutine = StartCoroutine(WarningSound());
        }

        while (true)
        {
            //アクションのUIが出ている場合点滅UIを非表示
            if (controllUiCanvas.activeSelf)
                blinkImage.enabled = false;
            else
                blinkImage.enabled = !blinkImage.enabled;

            yield return new WaitForSeconds(interval);

        }
    }

    IEnumerator WarningSound()
    {
        while (true)
        {
            SimpleAudioManager.PlayOneShot(warningSound);
            yield return new WaitForSeconds(0.5f);

        }
    }
}

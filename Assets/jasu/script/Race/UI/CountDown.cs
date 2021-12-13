using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour
{
    [SerializeField]
    float countDownTimeSeconds = 3f;
    
    float countDownTimer = 0f;

    [SerializeField]
    float liveTimeSeconds = 1f;

    float liveTimer = 0f;

    [SerializeField]
    float activeTimeSeconds = 3f;

    [SerializeField]
    string timeOutText = "Timeout";

    [SerializeField]
    Text text = null;

    public bool isTimeOut { get; private set; } = false;

    [SerializeField]
    GameObject deactiveObj;

    [SerializeField]
    AudioClip seCountDown;

    bool playedSe = false;

    // Start is called before the first frame update
    void Start()
    {
        countDownTimer = countDownTimeSeconds;
        text.text = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(countDownTimer > 0)
        {
            countDownTimer -= Time.deltaTime;
            if (text != null)
            {
                if((int)countDownTimer + 1 <= activeTimeSeconds)
                {
                    if (!playedSe)
                    {
                        playedSe = true;
                        SimpleAudioManager.PlayOneShot(seCountDown);
                    }
                    text.text = ((int)countDownTimer + 1).ToString();
                }
            }
        }
        else
        {
            isTimeOut = true;

            if (text != null)
            {
                text.text = timeOutText;
            }

            liveTimer += Time.deltaTime;
            if(liveTimer > liveTimeSeconds)
            {
                gameObject.SetActive(false);
                deactiveObj.SetActive(false);
            }
        }
    }
}

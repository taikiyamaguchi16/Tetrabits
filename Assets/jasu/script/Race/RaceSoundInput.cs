using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceSoundInput : MonoBehaviour
{
    bool leverOn = false;

    [SerializeField]
    AudioClip seButton;

    [SerializeField]
    AudioClip bgmLever;

    [SerializeField]
    AudioClip bgm;

    [SerializeField]
    RaceStart raceStart;

    [SerializeField]
    float bgmSwitchSeconds = 1f;

    float bgmSwitchTimer = 0f;

    bool bgmSwitched = true;

    // Update is called once per frame
    void Update()
    {
        if (raceStart.GetStarted())
        {
            //if (TetraInput.sTetraButton.GetTrigger())
            //{
            //    SimpleAudioManager.PlayOneShot(seButton);
            //}

            if (TetraInput.sTetraLever.GetPoweredOn())
            {
                if (!leverOn)
                {
                    leverOn = true;
                    if (bgmSwitched)
                    {
                        bgmSwitchTimer = 0f;
                        bgmSwitched = false;
                    }
                    else
                    {
                        bgmSwitched = true;
                    }
                }
                else
                {
                    if (!bgmSwitched)
                    {
                        bgmSwitchTimer += Time.deltaTime;
                        if (bgmSwitchTimer >= bgmSwitchSeconds)
                        {
                            bgmSwitched = true;
                            SimpleAudioManager.PlayBGMCrossFade(bgmLever, 1f);
                        }
                    }
                }
            }
            else
            {
                if (leverOn)
                {
                    leverOn = false;
                    if(bgmSwitched)
                    {
                        bgmSwitchTimer = 0f;
                        bgmSwitched = false;
                    }
                    else
                    {
                        bgmSwitched = true;
                    }
                }
                else
                {
                    if (!bgmSwitched)
                    {
                        bgmSwitchTimer += Time.deltaTime;
                        if (bgmSwitchTimer >= bgmSwitchSeconds)
                        {
                            bgmSwitched = true;
                            SimpleAudioManager.PlayBGMCrossFade(bgm, 1f);
                        }
                    }
                }
            }
        }
    }
}

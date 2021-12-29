using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceBGMCtrler : MonoBehaviour
{
    [SerializeField]
    AudioClip bgmLever;

    [SerializeField]
    AudioClip bgm;

    [SerializeField]
    ChaseRaceManager raceManager;

    bool leverOn = false;

    [SerializeField]
    float bgmSwitchSeconds = 1f;

    float bgmSwitchTimer = 0f;

    bool bgmSwitched = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (raceManager.started && !raceManager.goaled)
        {
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
                            SimpleAudioManager.PlayBGMCrossFade(bgm, 1f);
                        }
                    }
                }
            }
        }
    }
}

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

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger())
        {
            SimpleAudioManager.PlayOneShot(seButton);
        }

        if (TetraInput.sTetraLever.GetPoweredOn())
        {
            if (!leverOn)
            {
                leverOn = true;
                SimpleAudioManager.PlayBGMCrossFade(bgmLever, 1f);
            }
        }
        else
        {
            if (leverOn)
            {
                leverOn = false;
                SimpleAudioManager.PlayBGMCrossFade(bgm, 1f);
            }
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SatoBGM : MonoBehaviour
{
    [SerializeField]
    AudioClip bgmClip;

    [SerializeField]
    float Volume = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        SimpleAudioManager.PlayBGMCrossFade(bgmClip, 1.0f, Volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

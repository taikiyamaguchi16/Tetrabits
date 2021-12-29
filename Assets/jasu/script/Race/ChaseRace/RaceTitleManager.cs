using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceTitleManager : MonoBehaviour
{
    [SerializeField]
    AudioClip titleBgm;

    // Start is called before the first frame update
    void Start()
    {
        SimpleAudioManager.PlayBGMCrossFade(titleBgm, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

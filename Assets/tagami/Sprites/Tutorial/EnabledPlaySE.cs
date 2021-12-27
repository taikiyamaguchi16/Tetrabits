using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnabledPlaySE : MonoBehaviour
{
    [SerializeField] SEAudioClip seClip;

    private void OnEnable()
    {
        SimpleAudioManager.PlayOneShot(seClip);
    }
}

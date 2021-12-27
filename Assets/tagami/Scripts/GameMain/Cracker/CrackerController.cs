using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class CrackerController : MonoBehaviour
{
    [SerializeField] VisualEffect crackerEffect;
    [SerializeField] SEAudioClip crackerClip;

    public void PlayCracker()
    {
        crackerEffect.Play();
        SimpleAudioManager.PlayOneShot(crackerClip);
        return;
    }
}

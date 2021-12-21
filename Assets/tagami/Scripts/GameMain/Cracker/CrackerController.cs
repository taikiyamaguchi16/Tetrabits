using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;


public class CrackerController : MonoBehaviour
{
    [SerializeField] VisualEffect crackerEffect;

    public void PlayCracker()
    {
        crackerEffect.Play();
        return;
    }
}

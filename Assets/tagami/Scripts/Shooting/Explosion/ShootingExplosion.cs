using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingExplosion : MonoBehaviour
{
    [SerializeField] AudioClip explosionClip;

    private void Start()
    {
        SimpleAudioManager.PlayOneShot(explosionClip);
    }

    //Anim
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

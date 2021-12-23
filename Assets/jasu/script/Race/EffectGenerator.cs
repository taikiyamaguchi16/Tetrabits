using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject effectPrefab = null;

    [SerializeField]
    Transform effectInstanceTrans;

    [SerializeField]
    AudioClip effectAudioClip = null;

    public void InstanceEffect()
    {
        GameObject accelEffect = Instantiate(effectPrefab, effectInstanceTrans);
        accelEffect.transform.position = effectInstanceTrans.position;
        accelEffect.transform.localScale = effectInstanceTrans.localScale;

        if(effectAudioClip != null)
            SimpleAudioManager.PlayOneShot(effectAudioClip);
    }
}

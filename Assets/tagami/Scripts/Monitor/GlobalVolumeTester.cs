using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVolumeTester : MonoBehaviour
{
    [SerializeField] Color color;

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Rendering.VolumeProfile volumeProfile = GetComponent<UnityEngine.Rendering.Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(UnityEngine.Rendering.VolumeProfile));

        // You can leave this variable out of your function, so you can reuse it throughout your class.
        //UnityEngine.Rendering.Universal.Vignette vignette;
        UnityEngine.Rendering.Universal.ColorAdjustments colorAdjustments;

        if (!volumeProfile.TryGet(out colorAdjustments)) throw new System.NullReferenceException(nameof(colorAdjustments));

        colorAdjustments.colorFilter.value = color;

        //vignette.intensity.Override(intensity);
    }
}

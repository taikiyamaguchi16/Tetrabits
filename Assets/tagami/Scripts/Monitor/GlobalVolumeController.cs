using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Volume))]
public class GlobalVolumeController : MonoBehaviour
{
    //カラーをいじれるようにする
    public Color colorFilter
    {
        set { colorAdjustments.colorFilter.value = value; }
        get { return colorAdjustments.colorFilter.value; }
    }
    ColorAdjustments colorAdjustments;

    // Start is called before the first frame update
    void Start()
    {
        //プロファイルの取得
        VolumeProfile volumeProfile = GetComponent<Volume>()?.profile;
        if (!volumeProfile) throw new System.NullReferenceException(nameof(VolumeProfile));

        //コンポーネントの取得
        if (!volumeProfile.TryGet(out colorAdjustments)) throw new System.NullReferenceException(nameof(colorAdjustments));
    }
}

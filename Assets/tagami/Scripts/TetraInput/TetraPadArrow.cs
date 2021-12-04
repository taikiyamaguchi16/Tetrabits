using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPadArrow : MonoBehaviour
{
    Vector3 localScaleMax;

    // Start is called before the first frame update
    void Start()
    {
        localScaleMax = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //回転方向を決める
        var endRotation = Quaternion.FromToRotation(Vector3.right, (TetraInput.sTetraPad.GetVector()).normalized);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, endRotation, 0.5f);

        //Scaleを決める
        transform.localScale = Vector3.Lerp(transform.localScale, localScaleMax * TetraInput.sTetraPad.GetVector().magnitude * 0.25f, 0.5f);
    }
}

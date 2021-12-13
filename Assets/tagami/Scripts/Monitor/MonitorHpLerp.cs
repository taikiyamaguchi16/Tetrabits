using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorHpLerp : MonoBehaviour
{
    [SerializeField] bool usePosition;
    [SerializeField] bool useLocalScaleX;
    [SerializeField] Transform endTransform;

    [SerializeField] float lerpSingle;

    Vector3 startPosition;
    Vector3 startScale;

    // Start is called before the first frame update
    void Awake()
    {
        //StartだとMonitorに先越される
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    public void SetLerpSingle(float _single)
    {
        lerpSingle = _single;
        if (usePosition)
        {
            transform.position = Vector3.Lerp(endTransform.position, startPosition, lerpSingle);
        }
        if (useLocalScaleX)
        {
            var scale = transform.localScale;
            scale.x = Mathf.Lerp(endTransform.localScale.x, startScale.x, lerpSingle);
            transform.localScale = scale;
        }
    }
}

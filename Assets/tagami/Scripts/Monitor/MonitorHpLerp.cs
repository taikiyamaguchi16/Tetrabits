using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorHpLerp : MonoBehaviour
{
    [SerializeField] bool usePosition;
    [SerializeField] bool useLocalScaleX;
    [SerializeField] Transform endTransform;

    [SerializeField] float lerpSingle;
    float targetLerpSingle;

    Vector3 startPosition;
    Vector3 startScale;

    // Start is called before the first frame update
    void Awake()
    {
        //StartだとMonitorに先越される
        startPosition = transform.position;
        startScale = transform.localScale;
    }

    private void Update()
    {
        //少し遅れて
        lerpSingle = Mathf.Lerp(lerpSingle, targetLerpSingle, 0.5f);
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

    public void SetLerpSingle(float _single)
    {
        targetLerpSingle = _single;
    }
}

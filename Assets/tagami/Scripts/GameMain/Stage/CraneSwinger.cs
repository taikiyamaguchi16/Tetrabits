using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneSwinger : MonoBehaviour
{
    [SerializeField] AnimationCurve swingCurve;
    [SerializeField] float swingSeconds=1.0f;
    float swingTimer;
    [SerializeField] float swingMultiplier=1.0f;
    bool isSwing;

    Vector3 initialLocalPosition;

    private void Start()
    {
        initialLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSwing)
        {
            swingTimer += Time.deltaTime;
            if (swingTimer >= swingSeconds)
            {//終了
                isSwing = false;
                swingTimer = swingSeconds;
            }

            var offsetX = swingCurve.Evaluate(swingTimer / swingSeconds) * swingMultiplier;
            transform.localPosition = initialLocalPosition+new Vector3(offsetX,0.0f,0.0f);
        }
    }

    [ContextMenu("Swing")]
    public void Swing()
    {
        isSwing = true;
        swingTimer = 0.0f;
    }
}

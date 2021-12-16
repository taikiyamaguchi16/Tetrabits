using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Generic
{
    public class UIEasing : MonoBehaviour
    {
        [Header("Position")]
        [SerializeField] bool usePositionLerp;
        [SerializeField] Vector3 startLocalPosition;
        [SerializeField] Vector3 endLocalPosition;
        [SerializeField] float positionLerpSeconds = 1.0f;
        float positionLerpTimer;
        [HideInInspector] public float positionLerpTimeScale = 1.0f;
        [SerializeField] bool positionLerpRepeat;
        [SerializeField] AnimationCurve positionLerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Scale")]
        [SerializeField] bool useScaleLerp;
        [SerializeField] Vector3 startLocalScale = Vector3.one;
        [SerializeField] Vector3 endLocalScale = Vector3.one;
        [SerializeField] float scaleLerpSeconds = 1.0f;
        float scaleLerpTimer;
        [SerializeField] bool scaleLerpRepeat;
        [SerializeField] AnimationCurve scaleLerpCurve = AnimationCurve.Linear(0, 0, 1, 1);

        [Header("Debug")]
        [SerializeField] bool enabledTimerReset;

        private void Start()
        {
            if (usePositionLerp)
            {
                transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, positionLerpCurve.Evaluate(positionLerpTimer / positionLerpSeconds));
            }
            if (useScaleLerp)
            {
                transform.localScale = Vector3.Lerp(startLocalScale, endLocalScale, scaleLerpCurve.Evaluate(scaleLerpTimer / scaleLerpSeconds));
            }
        }

        // Update is called once per frame
        void Update()
        {
            //position
            if (usePositionLerp)
            {
                positionLerpTimer += Time.deltaTime * positionLerpTimeScale;
                if (positionLerpTimer > positionLerpSeconds)
                {
                    if (positionLerpRepeat)
                    {
                        positionLerpTimer = 0.0f;
                    }
                    else
                    {
                        positionLerpTimer = positionLerpSeconds;
                    }
                }
                transform.localPosition = Vector3.Lerp(startLocalPosition, endLocalPosition, positionLerpCurve.Evaluate(positionLerpTimer / positionLerpSeconds));
            }

            //scale
            if (useScaleLerp)
            {
                scaleLerpTimer += Time.deltaTime;
                if (scaleLerpTimer > scaleLerpSeconds)
                {
                    if (scaleLerpRepeat)
                    {
                        scaleLerpTimer = 0.0f;
                    }
                    else
                    {
                        scaleLerpTimer = scaleLerpSeconds;
                    }
                }
                transform.localScale = Vector3.Lerp(startLocalScale, endLocalScale, scaleLerpCurve.Evaluate(scaleLerpTimer / scaleLerpSeconds));
            }
        }//update

        private void OnEnable()
        {
            if (enabledTimerReset)
            {
                positionLerpTimer = 0.0f;
                scaleLerpTimer = 0.0f;
            }
        }

        public float GetPositionLerpSingle()
        {
            return positionLerpTimer / positionLerpSeconds;
        }

    }//class



}//namespace


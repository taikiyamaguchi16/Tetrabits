using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FluorescentLight : MonoBehaviour
{
   
    [SerializeField] AnimationCurve intensityCurve;
    [SerializeField] float flickeringSeconds = 1.0f;
    float intensityMultiplier;
    float flickeringTimer;
    bool isFlickering;

    Light myLight;

    // Start is called before the first frame update
    void Start()
    {
        myLight = GetComponent<Light>();
        intensityMultiplier = myLight.intensity;

        //StartFlickering();
    }

    private void OnEnable()
    {
        StartFlickering();
    }

    // Update is called once per frame
    void Update()
    {
        if (flickeringSeconds <= 0) return;

        if (isFlickering)
        {
            flickeringTimer += Time.deltaTime;
            if (flickeringTimer >= flickeringSeconds)
            {
                //終了
                isFlickering = false;
                flickeringTimer = 0.0f;
                myLight.intensity = intensityMultiplier;
            }
            else
            {
                //明るさを変更する
                myLight.intensity = intensityCurve.Evaluate(flickeringTimer / flickeringSeconds) * intensityMultiplier;
            }         
        }
    }

    void StartFlickering()
    {
        isFlickering = true;
    }
}

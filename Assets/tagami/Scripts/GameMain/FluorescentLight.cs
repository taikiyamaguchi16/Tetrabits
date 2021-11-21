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

    [SerializeField] bool occasionallyFlickering;
    [SerializeField] float OccasionallyFlickeringSeconds=10.0f;
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

        if (occasionallyFlickering)
        {
            StartCoroutine(OccasionallyFlickeringLoop(OccasionallyFlickeringSeconds));
        }
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

    public void StartFlickering()
    {
        isFlickering = true;
    }

    private IEnumerator OccasionallyFlickeringLoop(float _seconds)
    {
        // ループ
        while (true)
        {
            // secondで指定した秒数ループします
            yield return new WaitForSeconds(_seconds);
            StartFlickering();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTRamdomNoise : MonoBehaviour
{
    [SerializeField]
    CRT crt;

    [Header("調節")]

    [SerializeField, Tooltip("ノイズの間隔(秒)")]
    MinMaxFloat noiseInterval;

    [SerializeField, Tooltip("ノイズの継続時間")]
    MinMaxFloat noiseDuration;

    [SerializeField]
    MinMaxFloat noiseX;

    [SerializeField]
    MinMaxFloat rgbNoise;

    [SerializeField]
    MinMaxFloat sinNoiseScale;

    [SerializeField]
    MinMaxFloat sinNoiseWidth;

    [SerializeField]
    MinMaxFloat sinNoiseOffset;
    
    float timer = 0f;

    bool noising = false;

    public bool noiseActive = false;

    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(noiseInterval.min, noiseInterval.max);
    }

    // Update is called once per frame
    void Update()
    {
        if (noiseActive)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                if (noising)
                {
                    noising = false;
                    timer = Random.Range(noiseInterval.min, noiseInterval.max);

                    crt.NoiseX = 0f;
                    crt.RGBNoise = 0f;
                    crt.SinNoiseScale = 0f;
                    crt.SinNoiseWidth = 0f;
                    crt.SinNoiseOffset = 0f;
                }
                else
                {
                    noising = true;
                    timer = Random.Range(noiseDuration.min, noiseDuration.max);

                    crt.NoiseX = Random.Range(noiseX.min, noiseX.max);
                    crt.RGBNoise = Random.Range(rgbNoise.min, rgbNoise.max);
                    crt.SinNoiseScale = Random.Range(sinNoiseScale.min, sinNoiseScale.max);
                    crt.SinNoiseWidth = Random.Range(sinNoiseWidth.min, sinNoiseWidth.max);
                    crt.SinNoiseOffset = Random.Range(sinNoiseOffset.min, sinNoiseOffset.max);
                }
            }
        }
        else
        {
            crt.NoiseX = 0f;
            crt.RGBNoise = 0f;
            crt.SinNoiseScale = 0f;
            crt.SinNoiseWidth = 0f;
            crt.SinNoiseOffset = 0f;
        }

    }
}

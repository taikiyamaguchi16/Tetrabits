﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CRTNoise : MonoBehaviour
{
    [SerializeField]
    CRT crt;

    [Tooltip("ノイズ有効")]
    public bool noiseActive = true;

    [Tooltip("ランダムノイズ有効")]
    bool randomNoiseActive = true;

    [Header("常にノイズ")]

    [Tooltip("設定された継続時間でノイズを止める")]
    public bool stopNoiseInDuration = false;
    
    float noiseTimer = 0f;

    [SerializeField, Tooltip("ノイズの継続時間")]
    float defaultNoiseDuration = 3f;

    float noiseDuration = 3f;

    [SerializeField]
    float noiseX;

    [SerializeField]
    float rgbNoise;

    [SerializeField]
    float sinNoiseScale;

    [SerializeField]
    float sinNoiseWidth;

    [SerializeField]
    float sinNoiseOffset;

    [Header("ランダムノイズ")]

    [SerializeField, Tooltip("ランダムノイズの間隔(秒)")]
    MinMaxFloat randomNoiseInterval;

    [SerializeField, Tooltip("ランダムノイズの継続時間")]
    MinMaxFloat randomNoiseDuration;

    [SerializeField]
    MinMaxFloat noiseXRandom;

    [SerializeField]
    MinMaxFloat rgbNoiseRandom;

    [SerializeField]
    MinMaxFloat sinNoiseScaleRandom;

    [SerializeField]
    MinMaxFloat sinNoiseWidthRandom;

    [SerializeField]
    MinMaxFloat sinNoiseOffsetRandom;
    
    float randomNoiseTimer = 0f;

    bool noising = false;

    // 
    bool afterNoise = false;

    // Start is called before the first frame update
    void Start()
    {
        noiseTimer = defaultNoiseDuration;
        noiseDuration = defaultNoiseDuration;
        randomNoiseTimer = Random.Range(randomNoiseInterval.min, randomNoiseInterval.max);
    }

    // Update is called once per frame
    void Update()
    {
        if (noiseActive)
        {
            if (randomNoiseActive)  // ランダムノイズ
            {
                randomNoiseTimer -= Time.deltaTime;
                if (randomNoiseTimer <= 0f)
                {
                    if (noising)
                    {
                        noising = false;
                        randomNoiseTimer = Random.Range(randomNoiseInterval.min, randomNoiseInterval.max);

                        crt.NoiseX = 0f;
                        crt.RGBNoise = 0f;
                        crt.SinNoiseScale = 0f;
                        crt.SinNoiseWidth = 0f;
                        crt.SinNoiseOffset = 0f;
                    }
                    else
                    {
                        noising = true;
                        randomNoiseTimer = Random.Range(randomNoiseDuration.min, randomNoiseDuration.max);

                        crt.NoiseX = Random.Range(noiseXRandom.min, noiseXRandom.max);
                        crt.RGBNoise = Random.Range(rgbNoiseRandom.min, rgbNoiseRandom.max);
                        crt.SinNoiseScale = Random.Range(sinNoiseScaleRandom.min, sinNoiseScaleRandom.max);
                        crt.SinNoiseWidth = Random.Range(sinNoiseWidthRandom.min, sinNoiseWidthRandom.max);
                        crt.SinNoiseOffset = Random.Range(sinNoiseOffsetRandom.min, sinNoiseOffsetRandom.max);
                    }
                }
            }
            else　// 常にノイズ
            {
                if (stopNoiseInDuration)
                {
                    float halfNoiseDuration = noiseDuration / 2;
                    if (noiseTimer > halfNoiseDuration)
                    {
                        float multiply = 1 - (noiseTimer - halfNoiseDuration) / halfNoiseDuration;
                        crt.NoiseX = noiseX * multiply;
                        crt.RGBNoise = rgbNoise * multiply;
                        crt.SinNoiseScale = sinNoiseScale * multiply;
                        crt.SinNoiseWidth = sinNoiseWidth * multiply;
                        crt.SinNoiseOffset = sinNoiseOffset * multiply;
                    }
                    else
                    {
                        crt.NoiseX = noiseX * noiseTimer / halfNoiseDuration;
                        crt.RGBNoise = rgbNoise * noiseTimer / halfNoiseDuration;
                        crt.SinNoiseScale = sinNoiseScale * noiseTimer / halfNoiseDuration;
                        crt.SinNoiseWidth = sinNoiseWidth * noiseTimer / halfNoiseDuration;
                        crt.SinNoiseOffset = sinNoiseOffset * noiseTimer / halfNoiseDuration;
                    }
                    noiseTimer -= Time.deltaTime;

                    if (noiseTimer <= 0)
                    {
                        if (afterNoise)
                        {
                            RamdomNoise();
                        }
                        else
                        {
                            noiseActive = false;
                        }

                        stopNoiseInDuration = false;
                        
                        crt.NoiseX = 0f;
                        crt.RGBNoise = 0f;
                        crt.SinNoiseScale = 0f;
                        crt.SinNoiseWidth = 0f;
                        crt.SinNoiseOffset = 0f;
                    }
                }
                else
                {
                    crt.NoiseX = noiseX;
                    crt.RGBNoise = rgbNoise;
                    crt.SinNoiseScale = sinNoiseScale;
                    crt.SinNoiseWidth = sinNoiseWidth;
                    crt.SinNoiseOffset = sinNoiseOffset;
                }
            }

        }
        else // ノイズなし
        {
            crt.NoiseX = 0f;
            crt.RGBNoise = 0f;
            crt.SinNoiseScale = 0f;
            crt.SinNoiseWidth = 0f;
            crt.SinNoiseOffset = 0f;
        }
    }

    //public void SetRandomNoiseActive(bool _active)
    //{
    //    randomNoiseTimer = Random.Range(randomNoiseDuration.min, randomNoiseDuration.max);
    //    noiseTimer = noiseDuration;
    //    randomNoiseActive = _active;
    //}

    public void RamdomNoise()
    {
        noiseActive = true;
        randomNoiseActive = true;
        noising = false;
        randomNoiseTimer = Random.Range(randomNoiseDuration.min, randomNoiseDuration.max);

        crt.NoiseX = 0f;
        crt.RGBNoise = 0f;
        crt.SinNoiseScale = 0f;
        crt.SinNoiseWidth = 0f;
        crt.SinNoiseOffset = 0f;
    }

    public void AlWaysNoise()
    {
        noiseActive = true;
        randomNoiseActive = false;
        stopNoiseInDuration = false;
    }

    public void AlWaysNoiseWithTimeLimit(bool _afterNoise)
    {
        noiseActive = true;
        randomNoiseActive = false;
        stopNoiseInDuration = true;
        afterNoise = _afterNoise;
        noiseTimer = defaultNoiseDuration;
        noiseDuration = defaultNoiseDuration;
    }

    public void AlWaysNoiseWithTimeLimit(float _noiseDuration, bool _afterNoise)
    {
        AlWaysNoiseWithTimeLimit(_afterNoise);
        noiseTimer = _noiseDuration;
        noiseDuration = _noiseDuration;
    }

    public void SetNoiseActive(bool _active)
    {
        noiseActive = _active;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{
    CRTNoise noise;

    [SerializeField]
    [Header("Noiseを表示する時間")]
    float timeLimit = 1.0f;

    [SerializeField]
    [Header("trueでランダムノイズ有効化")]
    bool randomNoise = false;

    // Start is called before the first frame update
    void Start()
    {
        // コンポーネント取得
        noise = GameObject.Find("Display").GetComponent<CRTNoise>();

        noise.AlWaysNoiseWithTimeLimit(timeLimit, randomNoise);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

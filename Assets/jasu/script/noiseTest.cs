using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noiseTest : MonoBehaviour
{
    [SerializeField]
    CRTNoise crtNoise;

    [SerializeField]
    NoiseParameter noiseParam;

    // Start is called before the first frame update
    void Start()
    {
        crtNoise.AlWaysNoise();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            crtNoise.SetNoiseParam(noiseParam);
            crtNoise.AlWaysNoise();
        }
    }
}

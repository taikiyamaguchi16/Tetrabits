using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class noiseTest : MonoBehaviour
{
    [SerializeField]
    CRTNoise cRTNoise; 

    // Start is called before the first frame update
    void Start()
    {
        cRTNoise.AlWaysNoise();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            cRTNoise.AlWaysNoise();
        }
    }
}

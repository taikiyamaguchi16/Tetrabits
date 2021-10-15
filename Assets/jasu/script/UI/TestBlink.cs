using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestBlink : MonoBehaviour
{
    [SerializeField]
    Text text;

    [SerializeField]
    float changeValue = 0.1f;

    bool rise = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Color color = text.color;
        if (rise)
        {
            color.a -= changeValue;
            if (color.a <= 0f)
                rise = false;
        }
        else
        {
            color.a += changeValue;
            if (color.a >= 1f)
                rise = true;
        }
        text.color = color;
    }
}

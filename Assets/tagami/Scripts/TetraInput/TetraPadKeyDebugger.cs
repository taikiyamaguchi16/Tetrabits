using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPadKeyDebugger : TetraPad
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        padVector = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            padVector.y += 1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            padVector.y -= 1.0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            padVector.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            padVector.x += 1.0f;
        }
    }
}

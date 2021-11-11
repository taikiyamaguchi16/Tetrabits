using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraButtonKeyDebugger : TetraButton
{
    // Update is called once per frame
    void Update()
    {
        oldButtonState = buttonState;
        buttonState = Input.GetKey(KeyCode.E);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerRotater : MonoBehaviour
{
    Transform rotateTarget;

  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int xinputIndex = 0;

        if(XInputManager.GetButtonPress(xinputIndex,XButtonType.LThumbStickLeft))
        {
            //rotateTarget.rotation *= Quaternion.AngleAxis(,);
        }
        if (XInputManager.GetButtonPress(xinputIndex, XButtonType.LThumbStickRight))
        {

        }
        if (XInputManager.GetButtonPress(xinputIndex, XButtonType.LThumbStickUp))
        {

        }
        if (XInputManager.GetButtonPress(xinputIndex, XButtonType.LThumbStickDown))
        {

        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ControllerDetect : MonoBehaviour
{
    string[] CacheJoystickNames;
    bool ControllerFlag = false;
    int currentControllerNum = 0, oldControllerNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        CacheJoystickNames = Input.GetJoystickNames();

        for (int i = 0; i < CacheJoystickNames.Length; i++)
        {
            if (CacheJoystickNames[i] != "")
            {
                currentControllerNum++;
            }
        }

        if (currentControllerNum == 0)
        {
            Debug.Log("切断");
            ControllerFlag = false;
            Debug.Log(ControllerFlag);
        }
        else if (currentControllerNum > 0)
        {
            Debug.Log("接続");
            ControllerFlag = true;
            Debug.Log(ControllerFlag);
        }

        oldControllerNum = currentControllerNum;
        currentControllerNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        ConnectionDetect();
    }

    // 接続検知関数
    void ConnectionDetect()
    {
        CacheJoystickNames = Input.GetJoystickNames();

        for (int i = 0; i < CacheJoystickNames.Length; i++)
        {
            if (CacheJoystickNames[i] != "")
            {
                currentControllerNum++;
            }
        }

        if (currentControllerNum == 0 && oldControllerNum != currentControllerNum)
        {
            Debug.Log("切断");
            ControllerFlag = false;
            Debug.Log(ControllerFlag);
        }

        if (currentControllerNum > 0 && oldControllerNum != currentControllerNum)
        {
            Debug.Log("接続" + CacheJoystickNames.ToList()[0]);
            ControllerFlag = true;
            Debug.Log(ControllerFlag);
        }

        oldControllerNum = currentControllerNum;
        currentControllerNum = 0;
    }

    public bool GetControllerFlag()
    {
        return ControllerFlag;
    }
}

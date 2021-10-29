using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XInputAnyButton : MonoBehaviour
{
    private const int MAX_GAMEPAD = 4;

    private const int MAX_BUTTON = 21;

    public static bool GetAnyButtonTrigger()
    {
        for (int i = 0; i < MAX_GAMEPAD; i++)
        {
            for (int j = 0; j < MAX_BUTTON; j++)
            {
                if (XInputManager.GetButtonTrigger(i, (XButtonType)j))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool GetAnyButtonTrigger(int _index)
    {
        for (int i = 0; i < MAX_BUTTON; i++)
        {
            if (XInputManager.GetButtonTrigger(_index, (XButtonType)i))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetAnyButtonTrigger(XButtonType _type)
    {
        for (int i = 0; i < MAX_GAMEPAD; i++)
        {
            if (XInputManager.GetButtonTrigger(i, _type))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetAnyButtonPress()
    {
        for (int i = 0; i < MAX_GAMEPAD; i++)
        {
            for (int j = 0; j < MAX_BUTTON; j++)
            {
                if (XInputManager.GetButtonPress(i, (XButtonType)j))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public static bool GetAnyButtonPress(int _index)
    {
        for (int i = 0; i < MAX_BUTTON; i++)
        {
            if (XInputManager.GetButtonPress(_index, (XButtonType)i))
            {
                return true;
            }
        }
        return false;
    }

    public static bool GetAnyButtonPress(XButtonType _type)
    {
        for (int i = 0; i < MAX_GAMEPAD; i++)
        {
            if (XInputManager.GetButtonPress(i, _type))
            {
                return true;
            }
        }
        return false;
    }
}

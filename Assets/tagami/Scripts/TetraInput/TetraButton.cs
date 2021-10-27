﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraButton : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] GameObject buttonObj;
    [SerializeField] GameObject foundationObj;
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Option")]
    [SerializeField, Tooltip("ボタン押し返し力")] float ySpring = 100.0f;

    [System.NonSerialized]
    public bool keyDebug = false;

    bool buttonState;
    bool oldButtonState;

    // Start is called before the first frame update
    void Start()
    {
        //押し返し力設定
        var drive = foundationObj.GetComponent<ConfigurableJoint>().yDrive;
        drive.positionSpring = ySpring;
        foundationObj.GetComponent<ConfigurableJoint>().yDrive = drive;
    }

    // Update is called once per frame
    void Update()
    {
        //更新
        oldButtonState = buttonState;

        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || keyDebug)
        {
            //ベースより位置が低くなった時TRUE?
            buttonState = buttonObj.transform.position.y < foundationObj.transform.position.y || (keyDebug && Input.GetKey(KeyCode.E));
        }
        else
        {
            buttonState = false;
        }
    }

    public bool GetPress() { return buttonState; }

    public bool GetTrigger() { return buttonState && !oldButtonState; }

    public bool GetRelease() { return !buttonState && oldButtonState; }
}

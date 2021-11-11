using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraButton : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] ButtonBodyCollider buttonBodyCollider;
    [SerializeField] Rigidbody buttonRb;
    [SerializeField] ConfigurableJoint foundationJoint;
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Option")]
    [SerializeField, Tooltip("ボタン押し返し力")] float pressedYSpring = 100.0f;
    [SerializeField] float stdYSpring = 5000.0f;
    [SerializeField, Tooltip("この値よりボタンと土台のY値の差分が低くなった時ボタンが押されます")] float pressableDifferenceY = 0;

    [System.NonSerialized]
    public bool keyDebug = false;

    protected bool buttonState;
    protected bool oldButtonState;

    // Update is called once per frame
    void Update()
    {
        //更新
        oldButtonState = buttonState;

        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || keyDebug)
        {
            if (keyDebug)
            {
                buttonState = Input.GetKey(KeyCode.E);
            }
            else
            {
                buttonState = (buttonRb.transform.localPosition.y - foundationJoint.transform.localPosition.y) < pressableDifferenceY;
                if (GetTrigger())
                {
                    Debug.Log("ボタン.y-土台.y:" + (buttonRb.transform.localPosition.y - foundationJoint.transform.localPosition.y) + "< difference:" + pressableDifferenceY);
                }
            }
        }
        else
        {
            buttonState = false;
        }

        //押し返し力の設定
        if (buttonBodyCollider.collisionNum > 0)
        {
            SetYDrive(pressedYSpring);
        }
        else
        {
            SetYDrive(stdYSpring);
        }
    }

    private void SetYDrive(float _spring)
    {
        if (foundationJoint)
        {
            var drive = foundationJoint.yDrive;
            drive.positionSpring = _spring;
            foundationJoint.yDrive = drive;
        }
    }

    public bool GetPress() { return buttonState; }

    public bool GetTrigger() { return buttonState && !oldButtonState; }

    public bool GetRelease() { return !buttonState && oldButtonState; }
}

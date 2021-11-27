using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoolerRotater : MonoBehaviourPunCallbacks
{
    public MonitorCooler rotateTarget;
    Quaternion rotQt = Quaternion.identity;

    float rotateAnglePerSeconds = 45;

    int frameCount = 0;

    // Update is called once per frame
    void FixedUpdate()
    {
        //11/18 マスタークライアント以外が回転させることができなかった
        //Callで無理やりマスタークライアントに回転してもらうしかないんかな？

        if (photonView.IsMine)
        {
            int controlXinputIndex = 0;

            if (Input.GetKey(KeyCode.A) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickLeft))
            {
                rotQt *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.up);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime,rotateTarget.transform.up));
            }
            if (Input.GetKey(KeyCode.D) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickRight))
            {
                rotQt *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.up);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.up));
            }
            if (Input.GetKey(KeyCode.W) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickUp))
            {
                rotQt *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right));
            }
            if (Input.GetKey(KeyCode.S) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickDown))
            {
                rotQt *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right));
            }

            frameCount++;
            if (frameCount > 4)
            {
                frameCount = 0;
                rotateTarget.CallMultiplyRotation(rotQt);
                rotQt = Quaternion.identity;
            }

        }
    }
}

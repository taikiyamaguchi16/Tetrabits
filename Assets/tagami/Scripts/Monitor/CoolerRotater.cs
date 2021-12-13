using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoolerRotater : MonoBehaviourPunCallbacks
{
    public MonitorCooler monitorCooler;

    float rotX = 0.0f;
    float rotY = 0.0f;

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
                //rotQtY *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.up);
                rotY += -rotateAnglePerSeconds * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.D) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickRight))
            {
                //rotQtY *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.up);
                rotY += rotateAnglePerSeconds * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.W) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickUp))
            {
                //rotQtX *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.right);
                rotX += rotateAnglePerSeconds * Time.fixedDeltaTime;
            }
            if (Input.GetKey(KeyCode.S) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickDown))
            {
                //rotQtX *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.right);
                rotX += -rotateAnglePerSeconds * Time.fixedDeltaTime;
            }

            frameCount++;
            if (frameCount > 4)
            {
                frameCount = 0;
                monitorCooler.CallMultiplyRotation(rotX, rotY);
                //rotQtX = Quaternion.identity;
                //rotQtY = Quaternion.identity;
                rotX = 0.0f;
                rotY = 0.0f;
            }

        }
    }
}

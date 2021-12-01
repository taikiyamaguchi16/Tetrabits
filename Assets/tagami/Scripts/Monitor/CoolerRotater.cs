using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoolerRotater : MonoBehaviourPunCallbacks
{
    public MonitorCooler monitorCooler;

    Quaternion rotQtX = Quaternion.identity;
    Quaternion rotQtY = Quaternion.identity;

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
                rotQtY *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.up);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime,rotateTarget.transform.up));
            }
            if (Input.GetKey(KeyCode.D) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickRight))
            {
                rotQtY *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.up);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.up));
            }
            if (Input.GetKey(KeyCode.W) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickUp))
            {
                rotQtX *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.right);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right));
            }
            if (Input.GetKey(KeyCode.S) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickDown))
            {
                rotQtX *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, Vector3.right);
                //rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right));
            }

            frameCount++;
            if (frameCount > 4)
            {
                frameCount = 0;
                monitorCooler.CallMultiplyRotation(rotQtX, rotQtY);
                rotQtX = Quaternion.identity;
                rotQtY = Quaternion.identity;
            }

        }
    }
}

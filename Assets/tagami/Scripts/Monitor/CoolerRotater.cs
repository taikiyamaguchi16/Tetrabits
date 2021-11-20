using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class CoolerRotater : MonoBehaviourPunCallbacks
{
    public MonitorCooler rotateTarget;

    float rotateAnglePerSeconds = 45;

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
                //rotateTarget.rotation *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.deltaTime, Vector3.up);
                rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime,rotateTarget.transform.up));
            }
            if (Input.GetKey(KeyCode.D) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickRight))
            {
                //rotateTarget.rotation *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.deltaTime, Vector3.up);
                rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.up));
            }
            if (Input.GetKey(KeyCode.W) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickUp))
            {
                //rotateTarget.rotation *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.deltaTime,rotateTarget.right);
                rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right));
            }
            if (Input.GetKey(KeyCode.S) || XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickDown))
            {
                //rotateTarget.rotation *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.deltaTime, rotateTarget.right);
                rotateTarget.CallMultiplyRotation(Quaternion.AngleAxis(rotateAnglePerSeconds * Time.fixedDeltaTime, rotateTarget.transform.right));
            }
        }
    }
}

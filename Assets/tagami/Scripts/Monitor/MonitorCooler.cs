using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorCooler : MonoBehaviour, IPlayerAction
{
    [Header("Required Reference")]
    [SerializeField] MonitorManager monitorManager;
    [SerializeField] ParticleSystem coolingEffect;
    [SerializeField] Transform rotateTarget;

    [Header("Status")]
    [SerializeField] float repairMonitorPerSeconds = 0.1f;
    [SerializeField] float rotateAnglePerSeconds = 10.0f;

    int controlXinputIndex = 0;

    bool running = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (running)
        {
            if (!coolingEffect.isPlaying)
            {
                coolingEffect.Play();
                Debug.Log("Effect Play");
            }

            monitorManager.RepairMonitor(repairMonitorPerSeconds * Time.deltaTime);

            //Inputどうしよ
            if (XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickLeft))
            {
                rotateTarget.rotation *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.deltaTime, Vector3.up);
            }
            if (XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickRight))
            {
                rotateTarget.rotation *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.deltaTime, Vector3.up);
            }
            if (XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickUp))
            {
                rotateTarget.rotation *= Quaternion.AngleAxis(-rotateAnglePerSeconds * Time.deltaTime, Vector3.right);
            }
            if (XInputManager.GetButtonPress(controlXinputIndex, XButtonType.LThumbStickDown))
            {
                rotateTarget.rotation *= Quaternion.AngleAxis(rotateAnglePerSeconds * Time.deltaTime, Vector3.right);
            }

            //レイとばす
            Debug.DrawRay(rotateTarget.position, rotateTarget.forward, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(new Ray(rotateTarget.position,rotateTarget.forward), out hit, 1000.0f))
            {
                Debug.Log(hit.collider.gameObject.name);
                if(hit.collider.CompareTag("CoolingTarget"))
                {
                    Destroy(hit.collider.gameObject);
                }
            }

        }
        else
        {
            if (coolingEffect.isPlaying)
            {
                coolingEffect.Stop();
            }
        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        running = true;

        var playerMove = _desc.playerObj.GetComponent<PlayerMove>();
        if (playerMove)
        {
            controlXinputIndex = playerMove.controllerID;
        }
        else
        {
            Debug.LogError("PlayerにPlayerMoveのスクリプトがアタッチされてない");
        }
    }

    public void EndPlayerAction(PlayerActionDesc _desc)
    {
        running = false;
    }

    public int GetPriority()
    {
        return 50;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchingLaneInInput : MonoBehaviour
{
    [SerializeField]
    MoveBetweenLane moveBetweenLane;

    [SerializeField]
    float padInputRangeY = 0.35f;

    Vector3 padPos;

    [SerializeField]
    float padLength = 3.5f;

    [Header("デバッグ用")]

    [SerializeField]
    float inputY;

    private void Start()
    {
        padPos = TetraInput.sTetraPad.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        inputY = 0f;
        if(TetraInput.sTetraPad.GetNumOnPad() > 0)
        {
            List<GameObject> onPadObjList = TetraInput.sTetraPad.GetObjectsOnPad();

            List<float> yLengthList = new List<float>();

            foreach (GameObject onPad in onPadObjList)
            {
                yLengthList.Add(onPad.transform.position.z - padPos.z);
            }

            float total = 0;
            foreach (float yLength in yLengthList)
            {
                total += yLength;
            }

            inputY = total / yLengthList.Count / padLength;

            //inputY = TetraInput.sTetraPad.GetVector().y / TetraInput.sTetraPad.GetNumOnPad();
        }
            
        if (inputY < -padInputRangeY)
        {
            if(moveBetweenLane.belongingLaneId != 0)
            {
                moveBetweenLane.SetMoveLane(0);
            }
        }
        else if(inputY > padInputRangeY)
        {
            if (moveBetweenLane.belongingLaneId != 2)
            {
                moveBetweenLane.SetMoveLane(2);
            }
        }
        else
        {
            if (moveBetweenLane.belongingLaneId != 1)
            {
                moveBetweenLane.SetMoveLane(1);
            }
        }
    }
}

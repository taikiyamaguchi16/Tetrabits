using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUICursor : MonoBehaviour
{
    [SerializeField]
    RectTransform myrectTrans;

    [SerializeField]
    List<RectTransform> targetList = new List<RectTransform>();

    [SerializeField]
    float spaceToTarget = 0f;

    public int selectedNum = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetPosSelected();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) || XInputAnyButton.GetAnyButtonTrigger(XButtonType.LThumbStickUp))
        {
            selectedNum--;
            if (selectedNum < 0)
                selectedNum = 0;
            SetPosSelected();
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) || XInputAnyButton.GetAnyButtonTrigger(XButtonType.LThumbStickDown))
        {
            selectedNum++;
            if (selectedNum > targetList.Count - 1)
                selectedNum = targetList.Count - 1;
            SetPosSelected();
        }
    }

    private void SetPosSelected()
    {
        Vector3 pos = targetList[selectedNum].position;
        //pos.x -= (targetList[selectedNum].sizeDelta.x + spaceToTarget);
        pos.x -= spaceToTarget;
        myrectTrans.position = pos;
    }

    public GameObject GetSelectedObj()
    {
        return targetList[selectedNum].gameObject;
    }
}

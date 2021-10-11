using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// トリガー内のオブジェクトの数を数える
public class TriggerSensor : MonoBehaviour
{
    int inTriggerNum;   // オブジェクトのカウント

    bool existInTrigger = false; // トリガー内に何かしらのオブジェクトがあればtrue;

    private void Start()
    {
        inTriggerNum = 0;
    }

    private void Update()
    {
        if(inTriggerNum > 0)
        {
            existInTrigger = true;
        }
        else
        {
            existInTrigger = false;
            inTriggerNum = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        inTriggerNum++;
    }

    private void OnTriggerExit(Collider other)
    {
        inTriggerNum--;
    }

    public int GetInTriggerNum()
    {
        return inTriggerNum;
    }

    public bool GetExistInTrigger()
    {
        return existInTrigger;
    }
}

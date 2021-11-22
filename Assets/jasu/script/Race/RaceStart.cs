using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStart : MonoBehaviour
{
    [SerializeField]
    BikeCtrlWhenStartAndGoal[] bikeCtrlStartGoals;

    [SerializeField]
    CountDown countDown;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (countDown.isTimeOut && !started)
        {
            started = true;
            foreach(var bikeCtrlStartGoal in bikeCtrlStartGoals)
            {
                bikeCtrlStartGoal.SetActiveBeforeStart(true);
            }
            Debug.Log("スタート");
        }
    }
}

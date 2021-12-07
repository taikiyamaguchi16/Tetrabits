using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroGoalControllr : MonoBehaviour
{
    [SerializeField] GameObject GyroUI;
    GyroTimeLimitController gyroTimeLimitControllerComponent;

    // Start is called before the first frame update
    void Start()
    {
        if (GyroUI == null) GyroUI = GameObject.Find("GyroUI");
        gyroTimeLimitControllerComponent = GyroUI.GetComponent<GyroTimeLimitController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gyroTimeLimitControllerComponent.GoalAnimation();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceStart : MonoBehaviour
{
    [SerializeField]
    BikeCtrlWhenStartAndGoal bikeCtrlStartGoal;

    bool started = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !started)
        {
            started = true;
            bikeCtrlStartGoal.SetActiveBeforeStart(true);
            Debug.Log("スタート");
        }
    }
}

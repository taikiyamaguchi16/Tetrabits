using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerController : MonoBehaviour
{
    [SerializeField]
    ChaseRaceManager raceManager;

    public ChaseRaceManager GetChaseRaceManager() { return raceManager; }

    [SerializeField]
    Rigidbody rb;

    public Rigidbody GetRigidbody() { return rb; }

    [SerializeField]
    RacerMove racerMove;

    public RacerMove GetRacerMove() { return racerMove; }

    [SerializeField]
    RacerJump racerJump;

    public RacerJump GetRacerJump() { return racerJump; }

    [SerializeField]
    RacerLaneShift racerLaneShift;

    public RacerLaneShift GetRacerLaneShift() { return racerLaneShift; }

    [SerializeField]
    RacerGroundSensor racerGroundSensor;

    public RacerGroundSensor GetRacerGroundSensor() { return racerGroundSensor; }

    [SerializeField]
    RacerSlipSensor racerSlipSensor;

    public RacerSlipSensor GetRacerSlipSensor() { return racerSlipSensor; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

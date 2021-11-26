using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ShiftStateInfo
{
    string stateName;
    AIState state;
}

public abstract class AIState : MonoBehaviour
{
    //[SerializeField]
    //protected List<AIState> shiftStates = new List<AIState>();

    [SerializeField]
    protected StateAIManager manager;

    [SerializeField]
    protected StateToNameTable shiftStateTable;

    protected bool shift = false;

    // Start is called before the first frame update
    public virtual void StateStart()
    {
        
    }

    // Update is called once per frame
    public virtual void StateUpdate()
    {
        
    }

    public virtual void StateLateUpdate()
    {

    }

    public virtual void StateFixedUpdate()
    {

    }

    public virtual void StateEnd()
    {

    }

    public virtual bool CheckShiftCondition()
    {
        return false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StateToName : Serialize.KeyAndValue<string, AIState>
{
    public StateToName(string key, AIState value) : base(key, value) {}
}


[System.Serializable]
public class StateToNameTable : Serialize.TableBase<string, AIState, StateToName> { }


public class StateAIManager : MonoBehaviour
{
    //[SerializeField]
    //List<AIState> stateList = new List<AIState>();

    [SerializeField]
    StateToNameTable stateTable; 

    [SerializeField]
    AIState activeState = null;

    // Start is called before the first frame update
    void Start()
    {
        if (activeState != null)
            activeState.StateStart();
    }

    // Update is called once per frame
    void Update()
    {
        // �J�ڏ����`�F�b�N
        Dictionary<string, AIState> stateDic = stateTable.GetTable();
        if(stateDic.Count > 0)
        {
            foreach(var state in stateDic)
            {
                if (state.Value.CheckShiftCondition() && state.Value != activeState)  // �J�ڏ����𖞂����Ă�����J�ڂ���break
                { 
                    StateShift(state.Value);
                    break;
                }
            }
        }
        
        // �A�N�e�B�u�X�e�[�g�̍X�V
        if (activeState != null)
            activeState.StateUpdate();
    }

    private void LateUpdate()
    {
        if (activeState != null)
            activeState.StateLateUpdate();
    }

    private void FixedUpdate()
    {
        if (activeState != null)
            activeState.StateFixedUpdate();
    }

    // �X�e�[�g�J��
    public void StateShift(AIState aiState)
    {
        // ���݃X�e�[�g�̏I������
        if (activeState != null)
            activeState.StateEnd();

        // �w��̃X�e�[�g�ɑJ�ځA������
        activeState = aiState;
        activeState.StateStart();
    }
}

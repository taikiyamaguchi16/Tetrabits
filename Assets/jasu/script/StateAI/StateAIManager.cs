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
        // 遷移条件チェック
        Dictionary<string, AIState> stateDic = stateTable.GetTable();
        if(stateDic.Count > 0)
        {
            foreach(var state in stateDic)
            {
                if (state.Value.CheckShiftCondition() && state.Value != activeState)  // 遷移条件を満たしていたら遷移してbreak
                { 
                    StateShift(state.Value);
                    break;
                }
            }
        }
        
        // アクティブステートの更新
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

    // ステート遷移
    public void StateShift(AIState aiState)
    {
        // 現在ステートの終了処理
        if (activeState != null)
            activeState.StateEnd();

        // 指定のステートに遷移、初期化
        activeState = aiState;
        activeState.StateStart();
    }
}

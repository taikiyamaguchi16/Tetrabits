using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverZenmaiPower : MonoBehaviour, IPlayerAction
{
    Zenmai zenmai;  // ゼンマイ

    [SerializeField]
    float recoveryAmount = 0.02f;  // ゼンマイパワー回復量

    bool windFlag = false;  // 巻きフラグ

    // Start is called before the first frame update
    void Start()
    {
        zenmai = GetComponent<Zenmai>();
    }

    private void Update()
    {
        if (windFlag)   // ゼンマイを巻く
        {
            zenmai.zenmaiPower += recoveryAmount;
            if (zenmai.zenmaiPower > zenmai.maxZenmaiPower)
                zenmai.zenmaiPower = zenmai.maxZenmaiPower;
        }
    }

    // アクションインターフェース　スタート
    void IPlayerAction.StartPlayerAction(PlayerActionDesc _desc)
    {
        windFlag = true;
        zenmai.decreaseTrigger = false;
    }

    // アクションインターフェース　エンド
    void IPlayerAction.EndPlayerAction(PlayerActionDesc _desc)
    {
        windFlag = false;
        zenmai.decreaseTrigger = true;
    }

    int IPlayerAction.GetPriority()
    {
        return 50;
    }
}

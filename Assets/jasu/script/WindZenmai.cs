using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZenmai : MonoBehaviour, IPlayerAction
{
    Zenmai zenmai;

    [SerializeField]
    int windPerSeconds = 10;

    bool windFlag = false;

    // Start is called before the first frame update
    void Start()
    {
        zenmai = GetComponent<Zenmai>();
    }

    private void Update()
    {
        if (windFlag)
        {
            zenmai.zenmaiPower += windPerSeconds;
            if(zenmai.zenmaiPower > zenmai.maxZenmaiPower)
            {
                zenmai.zenmaiPower = zenmai.maxZenmaiPower;
            }
        }
    }

    void IPlayerAction.StartPlayerAction(PlayerActionDesc _desc)
    {
        windFlag = true;
    }

    void IPlayerAction.EndPlayerAction(PlayerActionDesc _desc)
    {
        windFlag = false;
    }
}

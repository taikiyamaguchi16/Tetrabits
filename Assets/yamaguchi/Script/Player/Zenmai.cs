using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Zenmai : MonoBehaviourPunCallbacks
{
    public float maxZenmaiPower = 100f;    // ゼンマイパワー最大値

    public float zenmaiPower; // ゼンマイパワー

    public float decrease = 0.01f;  // ゼンマイパワー減少値

    public bool decreaseTrigger;

    // Start is called before the first frame update
    void Start()
    {
        zenmaiPower = maxZenmaiPower;

        decreaseTrigger = false;
    }

    public void DecreaseZenmaiPower()
    {
        // ゼンマイパワー減少
        if (decreaseTrigger)
        {
            zenmaiPower -= decrease * Time.deltaTime * 300f;
            if (zenmaiPower < 0)
                zenmaiPower = 0;
        }
    }
}

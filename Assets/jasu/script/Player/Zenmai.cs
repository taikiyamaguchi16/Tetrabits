using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zenmai : MonoBehaviour
{
    public float maxZenmaiPower = 100f;    // ゼンマイパワー最大値

    public float zenmaiPower; // ゼンマイパワー

    public float decrease = 0.01f;  // ゼンマイパワー減少値

    public bool decreaseTrigger = true;

    // Start is called before the first frame update
    void Start()
    {
        zenmaiPower = maxZenmaiPower;
    }

    // Update is called once per frame
    void Update()
    {
        // ゼンマイパワー減少
        if (decreaseTrigger)
        {
            zenmaiPower -= decrease;
            if (zenmaiPower < 0)
                zenmaiPower = 0;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zenmai : MonoBehaviour
{
    public int maxZenmaiPower = 100;
    
    public int decreasePerSeconds = 1;

    public int zenmaiPower;

    float timer = 0f;

    // Start is called before the first frame update
    void Start()
    {
        zenmaiPower = maxZenmaiPower;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1 && zenmaiPower > 0)
        {
            timer = 0f;
            zenmaiPower -= decreasePerSeconds;
        }
    }
}

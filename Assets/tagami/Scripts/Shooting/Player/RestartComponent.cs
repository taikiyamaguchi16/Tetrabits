using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartComponent : MonoBehaviour
{

    float restartTimer;
    float restartSeconds;
    MonoBehaviour targetComponent = null;

    public void Restart(MonoBehaviour _restartComponent, float _restartSeconds)
    {
        targetComponent = _restartComponent;
        restartSeconds = _restartSeconds;
        restartTimer = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (targetComponent)
        {
            restartTimer += Time.deltaTime;
            if (restartTimer >= restartSeconds)
            {               
                targetComponent.enabled = true;
                targetComponent = null;
            }
        }
    }
}

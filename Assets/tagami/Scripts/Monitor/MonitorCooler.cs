using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonitorCooler : MonoBehaviour,IPlayerAction
{
    [Header("Required Reference")]
    [SerializeField] MonitorManager monitorManager;

    [Header("Status")]
    [SerializeField] float repairMonitorPerSeconds=0.1f;

    bool running = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(running)
        {
            monitorManager.RepairMonitor(repairMonitorPerSeconds * Time.deltaTime);
        }
    }

    public void StartPlayerAction(PlayerActionDesc _desc)
    {
        running = true;
    }

    public void EndPlayerAction(PlayerActionDesc _desc)
    {
        running = false;
    }
}

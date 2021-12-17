using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableEmissionGenerator : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] List<Transform> waypoints;

    [Header("Reference Battery Holder")]
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Generate")]
    [SerializeField] GameObject cableEmissionPrefab;
    [SerializeField] float generateIntervalSeconds = 5.0f;
    float generateTimer;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (batteryHolder.GetBatterylevel() > 0)
        {
            generateTimer += Time.fixedDeltaTime;
            if (generateTimer >= generateIntervalSeconds)
            {
                generateTimer = 0.0f;
                //生成
                var obj = Instantiate(cableEmissionPrefab);
                obj.GetComponent<CableEmission>().waypoints = waypoints;
            }
        }
    }
}

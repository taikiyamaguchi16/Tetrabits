using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CableEmission : MonoBehaviour
{
    //[Header("Waypoints")]
    [HideInInspector] public List<Transform> waypoints;
    float totalDistance;

    [Header("Status")]
    [SerializeField] float emissionSpeed = 1.0f;
    float emissionLerpSeconds;
    float emissionLerpTimer;

    private void Start()
    {
        //何秒かけて最後まで行くか
        totalDistance = Generic.Utility.MathfUtil.DistanceWaypoints(waypoints);
        emissionLerpSeconds = totalDistance / emissionSpeed;

        //位置の初期化
        transform.position = Generic.Utility.Vector3Util.LerpWaypoints(waypoints, 0);
    }

    private void FixedUpdate()
    {
        //Move
        emissionLerpTimer += Time.fixedDeltaTime;
        if (emissionLerpTimer >= emissionLerpSeconds)
        {
            emissionLerpTimer = emissionLerpSeconds;
            Destroy(gameObject);
        }
        transform.position = Generic.Utility.Vector3Util.LerpWaypoints(waypoints, emissionLerpTimer / emissionLerpSeconds);
    }
}

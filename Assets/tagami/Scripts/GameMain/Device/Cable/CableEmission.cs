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

        //rotation
        UpdateRotation();
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

        //rotation
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        //rotation
        var cameraV = Camera.main.transform.rotation * Vector3.forward;
        var reverse = cameraV * -1f;
        transform.localRotation = Quaternion.FromToRotation(Vector3.back, reverse) * Quaternion.AngleAxis(-90.0f, Vector3.right);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPad : MonoBehaviour
{
    [Header("Require External Reference")]
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Reference")]
    [SerializeField] TetraPadBody tetraPadBody;

    [System.NonSerialized]
    public bool deadBatteryDebug = false;

    protected Vector2 padVector;

    private void Update()
    {
        padVector = Vector2.zero;
        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || deadBatteryDebug)
        {
            //リストから合算ベクトルを作成           
            foreach (var obj in tetraPadBody.onPadObjects)
            {
                if (!obj) continue;

                var localVec = obj.transform.position - transform.position;
                localVec.Normalize();
                padVector.x += localVec.x;
                padVector.y += localVec.z;
            }
        }
    }

    public List<GameObject> GetObjectsOnPad()
    {
        if (tetraPadBody)
        {
            return tetraPadBody.onPadObjects;
        }
        return new List<GameObject>();
    }

    public Vector2 GetVector() { return padVector; }
}

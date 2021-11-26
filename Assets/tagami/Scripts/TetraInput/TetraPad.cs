using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPad : MonoBehaviour
{
    [Header("Require External Reference")]
    [SerializeField] BatteryHolder batteryHolder;

    [Header("Reference")]
    [SerializeField] TetraPadBody tetraPadBody;

    [Header("Status")]
    [SerializeField] float batteryConsumptionPerSeconds = 1.0f;

    [Header("Option")]
    [SerializeField] bool reverseVector;

    [System.NonSerialized]
    public bool deadBatteryDebug = false;

    protected Vector2 padVector;
    protected int numOnPad;

    private void Update()
    {
        numOnPad = 0;
        padVector = Vector2.zero;
        if ((batteryHolder && batteryHolder.GetBatterylevel() > 0) || deadBatteryDebug)
        {
            //パッドの人数を記録
            numOnPad = tetraPadBody.onPadObjects.Count;

            //リストから合算ベクトルを作成           
            foreach (var obj in tetraPadBody.onPadObjects)
            {
                if (!obj) continue;

                var localVec = obj.transform.position - transform.position;
                localVec.Normalize();
                if (reverseVector)
                {
                    padVector.x -= localVec.x;
                    padVector.y -= localVec.z;
                }
                else
                {
                    padVector.x += localVec.x;
                    padVector.y += localVec.z;
                }
            }
        }


        //電力消費
        if (numOnPad > 0)
        {
            batteryHolder.ConsumptionOwnBattery(batteryConsumptionPerSeconds * Time.deltaTime);
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

    public int GetNumOnPad() { return numOnPad; }
    public Vector2 GetVector() { return padVector; }
}

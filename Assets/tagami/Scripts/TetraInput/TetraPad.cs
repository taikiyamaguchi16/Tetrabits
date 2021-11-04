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
    public bool keyDebug = false;

    Vector2 padVector;

    private void Update()
    {
        padVector = Vector2.zero;

        if (batteryHolder && batteryHolder.GetBatterylevel() > 0)
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

        if (keyDebug)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                padVector.y += 1.0f;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                padVector.y -= 1.0f;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                padVector.x -= 1.0f;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                padVector.x += 1.0f;
            }
        }
    }

    public List<GameObject> GetObjectsOnPad() { return tetraPadBody.onPadObjects; }

    public Vector2 GetVector() { return padVector; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPad : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] TetraPadBody tetraPadBody;

    Vector2 padVector;

    private void Update()
    {
        //リストから合算ベクトルを作成
        padVector = Vector2.zero;
        foreach (var obj in tetraPadBody.padOnList)
        {
            var localVec = obj.transform.position - transform.position;
            localVec.Normalize();
            padVector.x += localVec.x;
            padVector.y += localVec.z;
        }
    }

    public Vector2 GetVector(){ return padVector; }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraPad : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] TetraPadBody tetraPadBody;

    [System.NonSerialized]
    public bool keyDebug = false;

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

        if(keyDebug)
        {
            padVector = Vector2.zero;
            if(Input.GetKey(KeyCode.UpArrow)|| Input.GetKey(KeyCode.W))
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

    public Vector2 GetVector(){ return padVector; }
}

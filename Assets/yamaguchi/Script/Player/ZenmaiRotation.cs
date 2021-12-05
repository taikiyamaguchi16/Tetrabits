using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZenmaiRotation : MonoBehaviour
{
    float zenmaiRotationSpeed;
    [SerializeField]
    Transform sidewaysPosition;

    private Vector3 backPosition;
    // Update is called once per frame

    Vector3 fowardVec;

    private bool recoverFg;

    private MeshRenderer zenmaiMesh;
    private void Awake()
    {
        backPosition=this.transform.localPosition;
        fowardVec = transform.forward;
        recoverFg = false;

        HideZenmaiMesh();
    }
    void Update()
    {
        float rotationSpeed = zenmaiRotationSpeed;
        if (recoverFg)
        {    
            rotationSpeed = -zenmaiRotationSpeed;
        }
        // x軸を軸にして毎秒2度、回転させるQuaternionを作成（変数をrotとする）
        Quaternion rot = Quaternion.AngleAxis(rotationSpeed, fowardVec);
        // 現在の自信の回転の情報を取得する。
        Quaternion q = this.transform.rotation;
        // 合成して、自身に設定
        this.transform.rotation = q * rot;
    }

    public void ChangeZenmaiPosition(bool _IsSideways)
    {
        //zenmaiMesh.enabled = true;
        if (_IsSideways)
        {
            this.transform.localPosition = sidewaysPosition.localPosition;
            Vector3 keepAngle = this.transform.localEulerAngles;
            keepAngle.y = 270f;
            this.transform.localEulerAngles = keepAngle;
        }
        else
        {
            this.transform.localPosition = backPosition;
            Vector3 keepAngle = this.transform.localEulerAngles;
            keepAngle.y = 0f;
            this.transform.localEulerAngles = keepAngle;
        }
    }

    public void HideZenmaiMesh()
    {
        Vector3 keepPos = backPosition;
        keepPos.z = 0.9f;
        this.transform.localPosition = keepPos;
        Vector3 keepAngle = this.transform.localEulerAngles;
        keepAngle.y = 180f;
        this.transform.localEulerAngles = keepAngle;
    }

    public void SetZenmaiRotationSpeed(float _speed)
    {
        zenmaiRotationSpeed = _speed;
    }

    public void SetRecoverFg(bool _fg)
    {
        recoverFg = _fg;
    }
}

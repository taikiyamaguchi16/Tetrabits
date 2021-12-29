using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Vector3 defaultScale;
    //どれだけ上に表示するか
    [SerializeField]
    private float upPosition;

    Vector3 def;

    [SerializeField]
    GameObject targetObj;
    // Start is called before the first frame update
    // Update is called once per frame

    private void Awake()
    {       
        defaultScale = targetObj.transform.lossyScale;
        def = targetObj.transform.localRotation.eulerAngles;
    }
    void Update()
    {
        Vector3 _parent = transform.transform.localRotation.eulerAngles;

        //修正箇所
        targetObj.transform.localRotation = Quaternion.Euler(def - _parent);


        Vector3 lossScale = targetObj.transform.lossyScale;
        Vector3 localScale = targetObj.transform.localScale;

        targetObj.transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z);

        Vector3 keepPos = targetObj.transform.parent.position;

        keepPos.y += upPosition;
        targetObj.transform.position = keepPos;

        Vector3 p = Camera.main.transform.position;
        p.y = targetObj.transform.position.y;
        targetObj.transform.LookAt(p);
    }
}

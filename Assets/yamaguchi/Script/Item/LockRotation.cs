using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour
{
    private Vector3 defaultScale;
    //どれだけ上に表示するか
    [SerializeField]
    private float upPosition;
    // Start is called before the first frame update
    // Update is called once per frame

    private void Awake()
    {       
        defaultScale = transform.lossyScale;
    }
    void Update()
    {
        if (this.transform.parent.localScale.z > 0f)
        {
            this.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        }
        Vector3 lossScale = transform.lossyScale;
        Vector3 localScale = transform.localScale;

        transform.localScale = new Vector3(
                localScale.x / lossScale.x * defaultScale.x,
                localScale.y / lossScale.y * defaultScale.y,
                localScale.z / lossScale.z * defaultScale.z);

        Vector3 keepPos = this.transform.parent.position;

        keepPos.y += upPosition;
        this.transform.position = keepPos;
    }
}

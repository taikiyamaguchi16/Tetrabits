using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanRotater : MonoBehaviour
{
    [SerializeField] float rotate = 30.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(Time.deltaTime * rotate, Vector3.right);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCameraController : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        if (cameraObject == null) cameraObject = GameObject.FindWithTag("MainCamera");
        cameraObject.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y,
            cameraObject.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        cameraObject.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y,
            cameraObject.transform.position.z);
        cameraObject.transform.rotation = this.transform.rotation;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeCamera : MonoBehaviour
{
    [SerializeField]
    GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        cameraObject.transform.position = transform.position;
        cameraObject.transform.rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //cameraObject.transform.position = transform.position;
        //cameraObject.transform.rotation = transform.rotation;
    }

    private void LateUpdate()
    {
        cameraObject.transform.position = transform.position;
        cameraObject.transform.rotation = transform.rotation;
    }
}

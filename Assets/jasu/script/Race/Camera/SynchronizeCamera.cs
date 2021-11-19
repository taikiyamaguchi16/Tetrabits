using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynchronizeCamera : MonoBehaviour
{
    [SerializeField]
    GameObject cameraObject;

    [SerializeField]
    bool selfSynchronise = false;

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
        if (selfSynchronise)
        {
            Synchronise();
        }
    }

    public void Synchronise()
    {
        cameraObject.transform.position = transform.position;
        cameraObject.transform.rotation = transform.rotation;
    }
}

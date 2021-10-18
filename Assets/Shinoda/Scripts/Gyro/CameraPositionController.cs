﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPositionController : MonoBehaviour
{
    [SerializeField] GameObject cameraObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraObject.transform.position = new Vector3(this.transform.position.x,
            this.transform.position.y,
            cameraObject.transform.position.z);
    }
}
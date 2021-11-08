﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBackground : MonoBehaviour
{
    [SerializeField] float scrollX = 1.0f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Mathf.Repeat(Time.time * scrollX, 1);
        Vector2 offset = new Vector2(scroll, 0);
        GetComponent<Renderer>().material.mainTextureOffset = offset;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTester : MonoBehaviour
{
    public static bool isRunning = true;

    [SerializeField] bool runUnitTest = true;

    // Start is called before the first frame update
    void Awake()
    {
        isRunning = runUnitTest;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

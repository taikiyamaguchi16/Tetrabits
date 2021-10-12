using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInGameManager : MonoBehaviour
{
    [SerializeField] List<GameObject> UnitTestObjects;

    // Start is called before the first frame update
    void Awake()
    {
        if (!UnitTester.isRunning)
        {
            DisactiveUnitTestObjects();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisactiveUnitTestObjects()
    {
        foreach(var obj in UnitTestObjects)
        {
            obj.SetActive(false);
        }
    }
}

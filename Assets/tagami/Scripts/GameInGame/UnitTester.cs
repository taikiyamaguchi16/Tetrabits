using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitTester : MonoBehaviour
{
    static bool occupancy = false;

    [Header("Reference")]
    [SerializeField] List<GameObject> UnitTestObjects;

    [Header("Option")]
    [SerializeField] bool completeInactiveUnitTest;

    // Start is called before the first frame update
    void Awake()
    {
        //ユニットテストの機能を切りたい場合
        if(completeInactiveUnitTest)
        {
            occupancy = true;   //占有状態にしておく
        }

        //占有状態の場合、UnitTest用の機能を落とす
        if (occupancy)
        {
            foreach (var obj in UnitTestObjects)
            {
                obj.SetActive(false);
            }
        }
    }
}

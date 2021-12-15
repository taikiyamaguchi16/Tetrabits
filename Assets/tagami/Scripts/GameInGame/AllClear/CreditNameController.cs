using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditNameController : MonoBehaviour
{
    [SerializeField] string creditName;

    // Start is called before the first frame update
    void Start()
    {
        if (creditName == null || creditName.Length <= 0)
        {
            Debug.LogError("クレジット名を設定してください");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TmpTest : MonoBehaviour
{
    [SerializeField]
    [Tooltip("TMP��text������")]
    TextMeshProUGUI Text = null;

    [SerializeField]
    Color color;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // �ԐF��
        Text.outlineColor = color;
    }
}

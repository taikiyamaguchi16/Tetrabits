using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TmpTest : MonoBehaviour
{
    [SerializeField]
    [Tooltip("TMPのtextを入れる")]
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
        // 赤色に
        Text.outlineColor = color;
    }
}

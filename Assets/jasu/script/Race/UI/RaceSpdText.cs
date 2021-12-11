using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceSpdText : MonoBehaviour
{
    Text text = null;

    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    float spd;

    // Start is called before the first frame update
    void Start()
    {
        if ((text = GetComponent<Text>()) == null)
        {
            Debug.Log("text取得失敗");
        }
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "spd: " + rb.velocity.magnitude.ToString("f0");
        spd = rb.velocity.magnitude;
    }
}

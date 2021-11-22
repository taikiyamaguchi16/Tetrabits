using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCoinBlock : MonoBehaviour
{
    JumpTimeController timeControllerComponent;

    // Start is called before the first frame update
    void Start()
    {
        timeControllerComponent = GameObject.Find("JumpUI").GetComponent<JumpTimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            timeControllerComponent.AddCoin();
        }
        Destroy(this.gameObject);
    }
}

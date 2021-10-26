using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GimmickController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    bool leverState = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(leverState!= TetraInput.sTetraLever.GetPoweredOn())
        {
            SwitchGimmick();
        }
        leverState = TetraInput.sTetraLever.GetPoweredOn();
    }

    void SwitchGimmick()
    {
        if(boxCollider.enabled)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.1f);
            boxCollider.enabled = false;
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
            boxCollider.enabled = true;
        }
    }
}
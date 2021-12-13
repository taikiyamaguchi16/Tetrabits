using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GyroCommonGimmick : MonoBehaviour
{
    [SerializeField] Sprite offSprite;
    [SerializeField] Sprite onSprite;

    [SerializeField] bool offStart = false;

    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    bool leverState = false;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        boxCollider = this.gameObject.GetComponent<BoxCollider2D>();

        if (offStart)
        {
            spriteRenderer.sprite = offSprite;
            boxCollider.enabled = false;
        }
        else
        {
            spriteRenderer.sprite = onSprite;
            boxCollider.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(TetraInput.sTetraButton.GetTrigger())
        {
            SwitchGimmick();
        }

        //if(leverState!= TetraInput.sTetraLever.GetPoweredOn())
        //{
        //    SwitchGimmick();
        //}
        //leverState = TetraInput.sTetraLever.GetPoweredOn();
    }

    void SwitchGimmick()
    {
        if (boxCollider.enabled)
        {
            spriteRenderer.sprite = offSprite;
            boxCollider.enabled = false;
        }
        else
        {
            spriteRenderer.sprite = onSprite;
            boxCollider.enabled = true;
        }
    }
}
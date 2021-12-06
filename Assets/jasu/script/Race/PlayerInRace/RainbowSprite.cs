using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RainbowSprite : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            spriteRenderer.color = Color.HSVToRGB(Time.time % 1f, 1f, 1f);
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
        if (!active)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }
}

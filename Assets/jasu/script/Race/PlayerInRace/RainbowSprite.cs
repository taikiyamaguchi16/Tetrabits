using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RainbowSprite : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer = null;

    public bool active { get; private set; } = false;

    [SerializeField]
    float matlight = 0.5f;

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
            spriteRenderer.material.SetFloat("_Additive", matlight);
        }
    }

    public void SetActive(bool _active)
    {
        active = _active;
        if (!active)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            spriteRenderer.material.SetFloat("_Additive", 0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBlink : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer spriteRenderer;

    [SerializeField]
    float changeValue = 0.05f;

    [SerializeField]
    float min = 0f;

    [SerializeField]
    float max = 1f;

    bool rise = false;

    bool actualActive;

    public bool active;

    // Start is called before the first frame update
    void Start()
    {
        Color color = spriteRenderer.color;
        color.a = min;
        spriteRenderer.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
            actualActive = true;

        if (actualActive)
        {
            Color color = spriteRenderer.color;
            if (rise)
            {
                color.a -= changeValue * Time.deltaTime;
                if (color.a < min)
                {
                    if (active)
                    {
                        color.a = min;
                        rise = false;
                    }
                    else
                    {
                        color.a = 0;
                        actualActive = false;
                    }
                }
            }
            else
            {
                color.a += changeValue * Time.deltaTime;
                if (color.a >= max)
                    rise = true;
            }
            spriteRenderer.color = color;
        }
    }
}

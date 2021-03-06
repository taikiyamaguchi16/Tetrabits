using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingBombField : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] AnimationCurve bombFieldCurve;
    [SerializeField] bool useAnimationCurve = true;

    [SerializeField] float lifeSeconds = 2.0f;
    float lifeTimer;
    [SerializeField] Vector3 startLocalScale = Vector3.zero;
    [SerializeField] Vector3 endLocalScale = Vector3.one;

    SpriteRenderer spriteRenderer;
    UnityEngine.UI.RawImage rawImage;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rawImage = GetComponent<UnityEngine.UI.RawImage>();

        transform.localScale = startLocalScale;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if (lifeTimer >= lifeSeconds)
        {
            lifeTimer = lifeSeconds;
            Destroy(gameObject);
        }

        float dt = 0;
        if (useAnimationCurve)
        {
            dt = bombFieldCurve.Evaluate(lifeTimer / lifeSeconds);
        }
        else
        {
            dt = lifeTimer / lifeSeconds;
        }


        transform.localScale = Vector3.Lerp(startLocalScale, endLocalScale, dt);

        if (spriteRenderer)
        {
            var color = spriteRenderer.color;
            color.a = 1 - dt;
            spriteRenderer.color = color;
        }
        else if (rawImage)
        {
            var color = rawImage.color;
            color.a = 1 - dt;
            rawImage.color = color;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            Destroy(collision.gameObject);
        }
    }
}

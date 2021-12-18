using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditBombFIeld : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] AnimationCurve bombFieldCurve;

    [SerializeField] float lifeSeconds = 2.0f;
    float lifeTimer;
    [SerializeField] Vector3 startLocalScale = Vector3.zero;
    [SerializeField] Vector3 endLocalScale = Vector3.one;

    [Header("impulse")]
    [SerializeField] float addForce;
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

        float dt = bombFieldCurve.Evaluate(lifeTimer / lifeSeconds);

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

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.attachedRigidbody)
        {
            collision.attachedRigidbody.AddForce((collision.transform.position - transform.position).normalized * addForce);
        }
    }
}

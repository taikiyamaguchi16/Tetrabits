using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody2D rigidbody;

    [SerializeField] float rotateScale = 0.1f;
    [SerializeField] float stopTime = 1.0f;
    [SerializeField] float gravityScale = 9.8f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();

        if (padVec.x > 0)
        {
            this.transform.Rotate(0.0f, 0.0f, 1.0f * rotateScale);
        }
        else if (padVec.x < 0)
        {
            this.transform.Rotate(0.0f, 0.0f, -1.0f * rotateScale);
        }

        if (TetraInput.sTetraButton.GetTrigger())
        {
            rigidbody.velocity = Vector2.zero;
        }


        rigidbody.AddForce(-(this.transform.up) * gravityScale);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerJump : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    [SerializeField]
    EffectGenerator jumpEffect;

    [SerializeField]
    Vector3 jumpVec;

    [SerializeField]
    float gravity = -100f;

    public bool jumpable = true;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = racerController.GetRigidbody();
    }

    // Update is called once per frame
    void Update()
    {
        if (TetraInput.sTetraButton.GetTrigger() && racerController.GetRacerGroundSensor().GetOnGround() && jumpable)
        {
            rb.AddForce(jumpVec, ForceMode.Impulse);
            jumpEffect.InstanceEffect();
        }
    }

    private void FixedUpdate()
    {
        if (!racerController.GetRacerGroundSensor().GetOnGround())
        {
            Vector3 gravityVec = Vector3.zero;
            gravityVec.y = gravity;
            rb.AddForce(gravityVec, ForceMode.Acceleration);
        }
    }
}

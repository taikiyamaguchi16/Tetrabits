using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSlipDown : MonoBehaviour
{
    public bool isSliping { get; private set; }

    [SerializeField]
    float slipingTimeSeconds = 2f;

    [SerializeField]
    float correctionTimeSeconds = 0.5f;

    [SerializeField]
    float slipSpd = 5f;

    float slipingTimer = 0f;

    Vector3 eulerWhenSlipStart;

    // Start is called before the first frame update
    void Start()
    {
        slipingTimeSeconds += correctionTimeSeconds;
    }

    // Update is called once per frame
    void Update()
    {
        if (isSliping)
        {
            slipingTimer += Time.deltaTime;
            transform.Rotate(0, slipSpd * (1 - slipingTimer / slipingTimeSeconds), 0);
            if (slipingTimer > slipingTimeSeconds - correctionTimeSeconds)
            {
                if (Mathf.Abs(transform.localEulerAngles.y - eulerWhenSlipStart.y) < 1f)
                {
                    transform.localEulerAngles = eulerWhenSlipStart;
                    isSliping = false;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SlipStart();
        }
    }

    public void SlipStart()
    {
        if (isSliping)
            return;

        isSliping = true;
        eulerWhenSlipStart = new Vector3(0, transform.localEulerAngles.y, 0);
        slipingTimer = 0f;
    }
}

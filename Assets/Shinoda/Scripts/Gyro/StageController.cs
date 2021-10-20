using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageController : MonoBehaviour
{
    [SerializeField] float rotateRatio = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 padVec = TetraInput.sTetraPad.GetVector();

        if (padVec.x > 0)
        {
            this.transform.Rotate(0.0f, 0.0f, -1.0f * rotateRatio);
        }
        else if (padVec.x < 0)
        {
            this.transform.Rotate(0.0f, 0.0f, 1.0f * rotateRatio);
        }
    }
}

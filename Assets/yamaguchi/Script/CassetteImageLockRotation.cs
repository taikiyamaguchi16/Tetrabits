using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CassetteImageLockRotation : MonoBehaviour
{  
    // Update is called once per frame
    void Update()
    {
        if(this.transform.parent.localScale.x<0f)
        {
            this.transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
        else
        {
            this.transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }
}

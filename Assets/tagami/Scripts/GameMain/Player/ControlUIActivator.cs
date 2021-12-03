using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlUIActivator : MonoBehaviour
{
    [SerializeField] GameObject controlUIObject;

    //Playerに呼んでもらう
    public void SetControlUIActive(bool _active)
    {
        controlUIObject.SetActive(_active);
    }
}

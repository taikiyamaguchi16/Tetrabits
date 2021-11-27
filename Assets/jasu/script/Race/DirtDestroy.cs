using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DirtDestroy : MonoBehaviourPunCallbacks
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bike")
        {
            Destroy(this.gameObject);
        }
    }
}

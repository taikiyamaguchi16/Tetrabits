using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DirtDestroy : MonoBehaviourPunCallbacks
{
    public RaceStageMolder raceStageMolder = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Bike")
        {
            DummyObj dummy;
            if ((dummy = GetComponent<DummyObj>()) != null)
            {
                Destroy(dummy.entity);
            }
            else
            {
                Destroy(this.gameObject);
            }
            raceStageMolder.GetDummyRoadMolder.DummyRoadMold();
        }
    }
}

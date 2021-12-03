using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class DirtInRace : MonoBehaviourPunCallbacks
{
    public RaceStageMolder raceStageMolder = null;

    [SerializeField, Tooltip("ぶつかったら消える")]
    bool destroyIfHitOther = true;

    private void OnTriggerEnter(Collider other)
    {
        if (destroyIfHitOther && other.gameObject.tag == "Bike")
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

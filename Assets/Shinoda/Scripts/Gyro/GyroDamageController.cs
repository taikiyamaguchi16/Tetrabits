using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GyroDamageController : MonoBehaviour
{
    [SerializeField] string damage = "small";

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (PhotonNetwork.IsMasterClient) MonitorManager.DealDamageToMonitor(damage);
    }
}
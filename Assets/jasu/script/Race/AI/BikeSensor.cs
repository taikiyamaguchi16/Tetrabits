using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeSensor : AISensor
{
    [SerializeField]
    List<GameObject> bikeList = new List<GameObject>();

    private void Update()
    {
        for (int i = 0; i < bikeList.Count; i++)
        {
            if (bikeList[i] == null)
            {
                bikeList.Remove(bikeList[i]);
            }
        }

        if (bikeList.Count <= 0)
        {
            sensorActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Bike")
        {
            bikeList.Add(other.transform.gameObject);
            sensorActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Bike")
        {
            bikeList.Remove(other.transform.gameObject);
            if (bikeList.Count <= 0)
            {
                bikeList.Clear();
                sensorActive = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtSensor : AISensor
{
    [SerializeField]
    List<GameObject> dirtList = new List<GameObject>();

    private void Update()
    {
        for (int i = 0; i < dirtList.Count; i++)
        {
            if (dirtList[i] == null)
            {
                dirtList.Remove(dirtList[i]);
            }
        }

        if(dirtList.Count <= 0)
        {
            sensorActive = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Dirt")
        {
            dirtList.Add(other.transform.gameObject);
            sensorActive = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Dirt")
        {
            dirtList.Remove(other.transform.gameObject);
            if (dirtList.Count <= 0)
            {
                dirtList.Clear();
                sensorActive = false;
            }
        }
    }
}

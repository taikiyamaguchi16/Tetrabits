using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GearsUI : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    [SerializeField]
    Image[] gearUIs;

    [SerializeField]
    Color activeColor;

    [SerializeField]
    Color inactiveColor;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int gear = racerController.GetRacerMove().moveGear;
        if (gear > gearUIs.Length) gear = gearUIs.Length;
        else if (gear < 0) gear = 0;

        foreach(var gearUI in gearUIs)
        {
            gearUI.color = inactiveColor;
        }

        for(int i = 0; i < gear; i++)
        {
            gearUIs[i].color = activeColor;
        }
    }
}

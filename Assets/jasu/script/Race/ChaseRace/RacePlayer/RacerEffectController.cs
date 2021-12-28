using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerEffectController : MonoBehaviour
{
    [SerializeField]
    RacerController racerController = null;

    [SerializeField]
    GameObject[] effects;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int gear = racerController.GetRacerMove().moveGear;

        foreach (var effect in effects)
        {
            effect.SetActive(false);
        }

        switch (gear)
        {
            case 0:
                break;
            case 1:
                effects[0].SetActive(true);
                break;
            case 2:
                effects[1].SetActive(true);
                break;
            case 3:
                effects[1].SetActive(true);
                effects[2].SetActive(true);
                break;
            case 4:
                effects[1].SetActive(true);
                effects[2].SetActive(true);
                effects[3].SetActive(true);
                break;
            default:
                break;

        }

    }
}

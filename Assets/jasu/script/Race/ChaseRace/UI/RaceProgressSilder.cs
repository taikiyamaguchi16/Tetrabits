using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaceProgressSilder : MonoBehaviour
{
    [SerializeField]
    RacerController racerController;

    [SerializeField]
    Transform start;

    [SerializeField]
    Slider slider;

    float laneLength;

    // Start is called before the first frame update
    void Start()
    {
        laneLength = racerController.GetChaseRaceManager().GetRaceStageMolder().GetLaneLength;
    }

    // Update is called once per frame
    void Update()
    {
        float progress = (racerController.transform.position.z - start.position.z) / laneLength;
        if (progress > 1f) progress = 1f;
        else if (progress < 0f) progress = 0f;
        slider.value = progress;
    }
}

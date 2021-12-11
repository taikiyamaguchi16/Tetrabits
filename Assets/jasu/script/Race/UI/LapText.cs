using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LapText : MonoBehaviour
{
    Text text = null;

    [SerializeField]
    RaceManager raceManager;

    [SerializeField]
    LapCounter playerLapCounter;

    // Start is called before the first frame update
    void Start()
    {
        if((text = GetComponent<Text>()) == null)
        {
            Debug.Log("text取得失敗");
        }
    }

    // Update is called once per frame
    void Update()
    {
        int lap = playerLapCounter.lapCount;
        if(lap <= 0)
        {
            lap = 1;
        }
        else if(lap > raceManager.GetLapNum)
        {
            lap = raceManager.GetLapNum;
        }

        text.text = "Lap: " + lap + "/" + raceManager.GetLapNum;
    }
}

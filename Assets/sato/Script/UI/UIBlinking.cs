using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIBlinking : DOManager
{
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        text = gameObject.GetComponent<Text>();

        text.DOFade(fadeTime, fadeRange).SetEase(easeTypes).SetLoops(loopTimes,loopTypes);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

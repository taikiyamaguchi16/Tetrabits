using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackerManager : MonoBehaviour
{
    [Header("Crackers")]
    [SerializeField] List<CrackerController> crackers;

    [Header("Play Status")]
    [SerializeField] float playCrackerIntervalSeconds = 1.0f;

    [ContextMenu("PlayCrackers")]
    public void PlayCrackers()
    {
        StartCoroutine(CoPlayCrackers());
    }
    IEnumerator CoPlayCrackers()
    {
        foreach(var cracker in crackers)
        {
            cracker.PlayCracker();
            yield return new WaitForSeconds(playCrackerIntervalSeconds);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BikeCtrlWhenStartAndGoal : MonoBehaviour
{
    [SerializeField, Tooltip("スタートしてからenableオン")]
    MonoBehaviour[] componentsOffBeforeStart;

    [SerializeField, Tooltip("ゴールしたあとenableオフ")]
    MonoBehaviour[] componentsOffAfterGoal;

    [SerializeField, Tooltip("ゴールしたあとenableオン")]
    MonoBehaviour[] componentsOnAfterGoal;

    // Start is called before the first frame update
    void Start()
    {
        SetActiveBeforeStart(false);
        SetActiveOnAfterGoal(false);
    }

    public void SetActiveBeforeStart(bool _active)
    {
        foreach (MonoBehaviour cpt in componentsOffBeforeStart)
        {
            cpt.enabled = _active;
        }
    }

    public void SetActiveOffAfterGoal(bool _active)
    {
        foreach (MonoBehaviour cpt in componentsOffAfterGoal)
        {
            cpt.enabled = _active;
        }
    }

    public void SetActiveOnAfterGoal(bool _active)
    {
        foreach (MonoBehaviour cpt in componentsOnAfterGoal)
        {
            cpt.enabled = _active;
        }
    }
}

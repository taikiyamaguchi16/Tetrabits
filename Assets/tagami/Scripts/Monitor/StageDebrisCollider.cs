using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDebrisCollider : MonoBehaviour, ICool
{
    [SerializeField] StageDebris stageDebris;

    public void OnCooled(float _damage)
    {
        stageDebris.OnCooled(_damage);
    }
}

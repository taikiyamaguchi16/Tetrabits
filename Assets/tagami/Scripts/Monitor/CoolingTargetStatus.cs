using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolingTargetStatus : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] float hpMax = 1.0f;
    float hp;

    [SerializeField] float DamageToMonitor;
    public float damageToMonitor { set { DamageToMonitor = value; } get { return DamageToMonitor; } }

    [Header("Option")]
    [SerializeField] UnityEngine.UI.Slider slider;

    private void Awake()
    {
        hp = hpMax;
        slider.maxValue = hpMax;
    }

    private void Update()
    {
        slider.value = hp / hpMax;
    }

    public bool TryToKill(float _damage)
    {
        hp -= _damage;
        if (hp <= 0)
        {
            return true;
        }

        return false;
    }
}

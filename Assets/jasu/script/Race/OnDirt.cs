using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDirt : MonoBehaviour
{
    public bool onDirt { get; set; } = false;

    [SerializeField]
    protected float dirtSlipSeconds = 2f;

    protected float dirtSlipTimer = 0f;
}

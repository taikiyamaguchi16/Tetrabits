using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(IndicatorColorData), menuName = "ScriptableObjects/GameMain/" + nameof(IndicatorColorData))]
public class IndicatorColorData : ScriptableObject
{
    [System.Serializable]
    public struct EmissionColor
    {
        public Color color;
        public float intensity;

        public Color CalcEmissionColor()
        {
            float factor = Mathf.Pow(2, intensity);
            return new Color(color.r * factor, color.g * factor, color.b * factor);
        }
    }

    [SerializeField] EmissionColor UsingColor;
    public EmissionColor usingColor { get { return UsingColor; } private set { UsingColor = value; } }

    [SerializeField] EmissionColor UsableColor;
    public EmissionColor usableColor { get { return UsableColor; } private set { UsableColor = value; } }

    [SerializeField] EmissionColor UnusableColor;
    public EmissionColor unusableColor { get { return UnusableColor; } private set { UnusableColor = value; } }


}

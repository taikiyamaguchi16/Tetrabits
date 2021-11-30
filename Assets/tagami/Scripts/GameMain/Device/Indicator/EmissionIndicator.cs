using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(Renderer))]
public class EmissionIndicator : MonoBehaviour
{
    [SerializeField] IndicatorColorData indicatorColorData;

    public enum ColorType
    {
        None,
        Using,
        Usable,
        Unusable
    }
    ColorType currentType = ColorType.None;

    Renderer myRenderer;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.material.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetColor(ColorType _type)
    {
        if (currentType == _type)
        {
            return;
        }

        //記録しておく
        currentType = _type;

        switch (_type)
        {
            case ColorType.Using:
                SetEmissionColor(indicatorColorData.usingColor);
                break;
            case ColorType.Usable:
                SetEmissionColor(indicatorColorData.usableColor);
                break;
            case ColorType.Unusable:
                SetEmissionColor(indicatorColorData.unusableColor);
                break;
            default:
                Debug.LogWarning("設定されていないIndicatorTypeです");
                break;
        }
    }

    private void SetEmissionColor(IndicatorColorData.EmissionColor _emissionColor)
    {
        float factor = Mathf.Pow(2, _emissionColor.intensity);
        var color = new Color(_emissionColor.color.r * factor, _emissionColor.color.g * factor, _emissionColor.color.b * factor);
        myRenderer.material.SetColor("_EmissionColor", color);
    }
}

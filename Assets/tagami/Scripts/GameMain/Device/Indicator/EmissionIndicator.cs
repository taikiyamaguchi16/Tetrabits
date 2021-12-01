using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class EmissionIndicator : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] IndicatorColorData indicatorColorData;

    public enum ColorType
    {
        None,
        Using,
        Usable,
        Unusable
    }
    ColorType currentType = ColorType.None;
    IndicatorColorData.EmissionColor currentEmissionColor;

    Renderer myRenderer;

    //startUp
    [HideInInspector] public bool startUpOccupancy = false;
    bool startUp;
    float startUpTimer;
    float startUpSeconds = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<Renderer>();
        myRenderer.material.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void Update()
    {
        if (startUp)
        {
            startUpTimer += Time.deltaTime;
            if (startUpTimer >= startUpSeconds)
            {
                startUpTimer = startUpSeconds;
                startUp = false;
                startUpOccupancy = false;
            }
        }
        if (startUpOccupancy)
        {
            if (currentType != ColorType.None)
            {
                //色をあげてく
                SetEmissionColor(Color.Lerp(Color.black, currentEmissionColor.CalcEmissionColor(), startUpTimer / startUpSeconds));            }
            else
            {
                SetEmissionColor(Color.black);
            }
        }
    }

    public void StartUpEmissionIndicator()
    {
        startUp = true;
        startUpTimer = 0.0f;
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
                currentEmissionColor = indicatorColorData.usingColor;
                break;
            case ColorType.Usable:
                currentEmissionColor = indicatorColorData.usableColor;
                break;
            case ColorType.Unusable:
                currentEmissionColor = indicatorColorData.unusableColor;
                break;
            default:
                Debug.LogWarning("設定されていないIndicatorTypeです");
                break;
        }

        if (!startUpOccupancy)
        {//実際に色を設定する
            SetEmissionColor(currentEmissionColor.CalcEmissionColor());
        }
    }

    private void SetEmissionColor(Color _color)
    {
        myRenderer.material.SetColor("_EmissionColor", _color);
    }
}

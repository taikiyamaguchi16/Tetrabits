using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryBarSlider : MonoBehaviour
{
    [Header("Body")]
    [SerializeField] GameObject barBody;
    Vector3 bodyScaleMax;
    [SerializeField] Gradient bodyColorGradient;
    [SerializeField] float bodyColorIntensity = 1.0f;
    Renderer bodyRenderer;


    public float value01;

    // Start is called before the first frame update
    void Start()
    {
        bodyScaleMax = barBody.transform.localScale;
        bodyRenderer = barBody.GetComponent<Renderer>();
        bodyRenderer.material.EnableKeyword("_EMISSION");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (value01 <= 0)
        {
            barBody.SetActive(false);
        }
        else
        {
            barBody.SetActive(true);
            barBody.transform.localScale = Vector3.Lerp(new Vector3(bodyScaleMax.x, 0, bodyScaleMax.z), bodyScaleMax, value01);
        }
        bodyRenderer.material.SetColor("_EmissionColor", Generic.Rendering.ColorUtil.EmissionColor(bodyColorGradient.Evaluate(value01), bodyColorIntensity));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoolerEnergyTank : MonoBehaviour
{
    [Header("Water")]
    [SerializeField] Transform waterTransform;
    Vector3 waterLocalScaleMax;
    [SerializeField] float waterLiterMax = 100.0f;
    [SerializeField] float waterLiter = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        waterLocalScaleMax = waterTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        //Scale変更
        waterTransform.localScale = Vector3.Lerp(new Vector3(waterLocalScaleMax.x,0.0f,waterLocalScaleMax.z), waterLocalScaleMax, waterLiter / waterLiterMax);
    }
}

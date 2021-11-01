using UnityEngine;
using System.Collections;

[ExecuteInEditMode()]
public class CRT : MonoBehaviour
{

    [SerializeField]
    Material material;

    [SerializeField]
    [Range(0, 1)]
    float noiseX;
    public float NoiseX { get { return noiseX; } set { noiseX = value; } }

    [SerializeField]
    [Range(0, 1)]
    float rgbNoise;
    public float RGBNoise { get { return rgbNoise; } set { rgbNoise = value; } }

    [SerializeField]
    [Range(0, 1)]
    float sinNoiseScale;
    public float SinNoiseScale { get { return sinNoiseScale; } set { sinNoiseScale = value; } }

    [SerializeField]
    [Range(0, 10)]
    float sinNoiseWidth;
    public float SinNoiseWidth { get { return sinNoiseWidth; } set { sinNoiseWidth = value; } }

    [SerializeField]
    float sinNoiseOffset;
    public float SinNoiseOffset { get { return sinNoiseOffset; } set { sinNoiseOffset = value; } }

    [SerializeField]
    Vector2 offset;
    public Vector2 Offset { get { return offset; } set { offset = value; } }

    [SerializeField]
    [Range(0, 1)]
    float scanLineBright = 0.5f;
    public float ScanLineBright { get { return scanLineBright; } set { scanLineBright = value; } }

    [SerializeField]
    [Range(0, 2)]
    float scanLineTail = 1.85f;
    public float ScanLineTail { get { return scanLineTail; } set { scanLineTail = value; } }

    [SerializeField]
    [Range(-10, 10)]
    float scanLineSpeed = 0.5f;
    public float ScanLineSpeed { get { return scanLineSpeed; } set { scanLineSpeed = value; } }

    [SerializeField]
    float darknessAtEdge = 1.2f;
    public float DarknessAtEdge { get { return darknessAtEdge; } set { darknessAtEdge = value; } }

    // Use this for initialization
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        material.SetFloat("_NoiseX", noiseX);
        material.SetFloat("_RGBNoise", rgbNoise);
        material.SetFloat("_SinNoiseScale", sinNoiseScale);
        material.SetFloat("_SinNoiseWidth", sinNoiseWidth);
        material.SetFloat("_SinNoiseOffset", sinNoiseOffset);
        material.SetFloat("_ScanLineSpeed", scanLineSpeed);
        material.SetFloat("_ScanLineTail", scanLineTail);
        material.SetVector("_Offset", offset);
        Graphics.Blit(src, dest, material);
    }

    private void Update()
    {
        material.SetFloat("_NoiseX", noiseX);
        material.SetFloat("_RGBNoise", rgbNoise);
        material.SetFloat("_SinNoiseScale", sinNoiseScale);
        material.SetFloat("_SinNoiseWidth", sinNoiseWidth);
        material.SetFloat("_SinNoiseOffset", sinNoiseOffset);
        material.SetFloat("_ScanLineSpeed", scanLineSpeed);
        material.SetFloat("_ScanLineTail", scanLineTail);
        material.SetFloat("_ScanLineBright", scanLineBright);
        material.SetVector("_Offset", offset);
        material.SetFloat("_DarknessAtEdge", darknessAtEdge);
    }
}

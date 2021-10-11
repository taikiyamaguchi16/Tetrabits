using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct ColorInRatio
{
    [Tooltip("割合(0~1)")] public float ratio;
    [Tooltip("UIカラー")] public Color color;
}

public class ShowZenmaiPower : MonoBehaviour
{
    [SerializeField]
    Zenmai zenmai;

    [SerializeField]
    Slider slider;

    [SerializeField]
    List<ColorInRatio> colorInRatios = new List<ColorInRatio>();

    [SerializeField]
    Image fillImage;

    // Start is called before the first frame update
    void Start()
    {
        slider.maxValue = zenmai.maxZenmaiPower;
        slider.value = zenmai.maxZenmaiPower;

        // ソート
        for (int i = 0; i < colorInRatios.Count; i++)
        {
            for (int j = i + 1; j < colorInRatios.Count; j++)
            {
                if (colorInRatios[i].ratio < colorInRatios[j].ratio)
                {
                    ColorInRatio tmp = colorInRatios[i];
                    colorInRatios[i] = colorInRatios[j];
                    colorInRatios[j] = tmp;
                }
            }
        }
    }

    private void LateUpdate()
    {
        slider.value = zenmai.zenmaiPower;  // スライダーに値を適用

        // ゼンマイパワーからカラー決定
        float ratio = zenmai.zenmaiPower / zenmai.maxZenmaiPower;
        foreach (var colorInRatio in colorInRatios)
        {
            if (ratio <= colorInRatio.ratio)
                fillImage.color = colorInRatio.color;
        }
    }
}

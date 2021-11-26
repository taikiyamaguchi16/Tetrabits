using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMainStartUp : MonoBehaviour
{

    //テスト中
    [SerializeField] Material mat;
    float intensity = 6.0f;

    [Header("Material Start Up")]
    [SerializeField] Color initialColor = new Color(0, 0, 0, 1);
    [SerializeField] float materialLerpSeconds = 1.0f;
    [SerializeField] List<Renderer> startUpRenderers;

    [Header("Enable Lights")]
    [SerializeField] List<Light> startUpLights;

    [Header("Other Events")]
    [SerializeField] UnityEvent startUpEvent;

    public void StartUpGameMain()
    {
        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        //カラーの一時保存、値を全て0にする
        List<Color> colorBuff = new List<Color>();
        foreach (var renderer in startUpRenderers)
        {
            //保存
            var mat = renderer.material;
            mat.EnableKeyword("_EMISSION");
            colorBuff.Add(mat.GetColor("_EmissionColor"));
            //値を0にする
            mat.SetColor("_EmissionColor", initialColor);
        }


        //カメラ引くのちょっと待つ
        yield return new WaitForSeconds(1.0f);

        //マテリアルの色あげてく処理
        bool materialLerpLoop = true;
        float timer = 0.0f;
        while (materialLerpLoop)
        {
            timer += Time.deltaTime;
            float dt = timer / materialLerpSeconds;
            if (timer >= materialLerpSeconds)
            {
                timer = materialLerpSeconds;
                materialLerpLoop = false;
            }

            //マテリアルLerp
            for (int i = 0; i < startUpRenderers.Count; i++)
            {
                startUpRenderers[i].material.SetColor("_EmissionColor", Color.Lerp(initialColor, colorBuff[i], dt));
            }

            yield return null;
        }

        yield return new WaitForSeconds(1.0f);

        //電気をつける
        foreach (var light in startUpLights)
        {
            light.gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);

        //その他起動処理
        startUpEvent.Invoke();

    }
}

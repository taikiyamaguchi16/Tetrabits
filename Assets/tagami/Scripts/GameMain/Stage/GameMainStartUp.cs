using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameMainStartUp : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] Trisibo.SceneField tutorialScene;

    [Header("Material Start Up")]
    [SerializeField] Color initialColor = new Color(0, 0, 0, 1);
    [SerializeField] float materialLerpSeconds = 1.0f;
    [SerializeField] List<Renderer> startUpRenderers;
    [SerializeField] List<EmissionIndicator> emissionIndicators;


    [Header("Enable Lights")]
    [SerializeField] List<Light> startUpLights;

    [Header("Disable Lights")]
    [SerializeField] List<Light> disableLights;

    [Header("Other Events")]
    [SerializeField] UnityEvent startUpEvent;

    [Header("Debug")]
    [SerializeField] List<Color> colorBuff = new List<Color>();

    private void Start()
    {
        //カラーの一時保存、値を全て0にする
        foreach (var renderer in startUpRenderers)
        {
            //保存
            var mat = renderer.material;
            mat.EnableKeyword("_EMISSION");
            colorBuff.Add(mat.GetColor("_EmissionColor"));
            //値を0にする
            mat.SetColor("_EmissionColor", initialColor);
        }

        //インジケーターを占有状態にする
        foreach (var indicator in emissionIndicators)
        {
            indicator.startUpOccupancy = true;
        }
    }

    public void StartUpGameMain()
    {
        StartCoroutine(StartUp());
    }

    IEnumerator StartUp()
    {
        //バッテリーの出現をスタート
        var batterySpoawnerObj = GameObject.Find("BatterySpawner");
        batterySpoawnerObj.GetComponent<BatterySpowner>().StartSpawn();

        //カメラ引く
        VirtualCameraManager.OnlyActive(1);

        //チュートリアル起動
        if (Photon.Pun.PhotonNetwork.IsMasterClient)
        {
            GameInGameUtil.SwitchGameInGameScene(GameInGameUtil.GetSceneNameByBuildIndex(tutorialScene.BuildIndex));
        }

        //電気消す
        foreach (var light in disableLights)
        {
            light.gameObject.SetActive(false);
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

        //インジケーターの色あげてく
        foreach (var indicator in emissionIndicators)
        {
            indicator.StartUpEmissionIndicator();
        }

        //色あがりきるの待つ
        yield return new WaitForSeconds(1.0f);

        //電気をつける
        foreach (var light in startUpLights)
        {
            light.gameObject.SetActive(true);
        }

        // yield return new WaitForSeconds(1.0f);

        //その他起動処理
        startUpEvent.Invoke();

    }
}

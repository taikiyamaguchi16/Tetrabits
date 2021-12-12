using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class SimpleAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource seAudioSource;
    [SerializeField] AudioSource bgmAudioSourceA;
    [SerializeField] AudioSource bgmAudioSourceB;

    AudioSource currentBGMAudioSource;
    AudioSource nextBGMAudioSource;

    bool crossFadeBGMToA;

    bool crossFading;
    float crossFadeTimer = 0;
    float crossFadeSeconds;

    [Header("Debug")]
    [SerializeField] AudioClip bgmA;
    [SerializeField] AudioClip bgmB;


    // Start is called before the first frame update
    void Start()
    {
        //unitとの関係でStartに書く
        sSimpleAudioManager = this;
    }

    private void Update()
    {
        if (crossFading)
        {
            crossFadeTimer += Time.deltaTime;
            if (crossFadeTimer >= crossFadeSeconds)
            {
                //終了
                crossFadeTimer = crossFadeSeconds;
                crossFading = false;
                currentBGMAudioSource.Stop();
            }

            if (crossFadeTimer <= 0.0f)
            {
                currentBGMAudioSource.volume = 0;
                nextBGMAudioSource.volume = 1;
            }
            else
            {
                currentBGMAudioSource.volume = 1 - (crossFadeTimer / crossFadeSeconds);
                nextBGMAudioSource.volume = (crossFadeTimer / crossFadeSeconds);
            }
        }

        //if (bgmA && bgmB)
        //{
        //    if (Input.GetKeyDown("a"))
        //    {
        //        PlayBGMCrossFade(bgmA, 0.0f);
        //    }
        //    if (Input.GetKeyDown("d"))
        //    {
        //        PlayBGMCrossFade(bgmB, 0.0f);
        //    }
        //}
    }

    AudioSource GetSEAudioSource()
    {
        return seAudioSource;
    }

    void MPlayBGMCrossFade(AudioClip _bgmClip, float _crossFadeSeconds)
    {
        //初期設定
        crossFading = true;
        crossFadeTimer = 0.0f;
        crossFadeSeconds = _crossFadeSeconds;

        if (crossFadeBGMToA)
        {
            currentBGMAudioSource = bgmAudioSourceB;
            nextBGMAudioSource = bgmAudioSourceA;
        }
        else
        {
            currentBGMAudioSource = bgmAudioSourceA;
            nextBGMAudioSource = bgmAudioSourceB;
        }
        //切り替え
        crossFadeBGMToA = !crossFadeBGMToA;

        //Clip登録&再生
        nextBGMAudioSource.clip = _bgmClip;
        nextBGMAudioSource.volume = 0;
        nextBGMAudioSource.Play();
    }

    //*********************************************************
    static SimpleAudioManager sSimpleAudioManager;

    public static void PlayOneShot(AudioClip _audioClip)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.GetSEAudioSource()?.PlayOneShot(_audioClip);
    }

    public static void PlayBGMOverride(AudioClip _bgmClip)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.MPlayBGMCrossFade(_bgmClip, 0.0f);
    }

    public static void PlayBGMCrossFade(AudioClip _bgmClip, float _crossFadeSeconds)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.MPlayBGMCrossFade(_bgmClip, _crossFadeSeconds);
    }

}

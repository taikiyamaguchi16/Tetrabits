using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SEAudioClip
{
    public AudioClip clip;
    public float volumeScale = 1.0f;
}

[DefaultExecutionOrder(-1)]
public class SimpleAudioManager : MonoBehaviour
{
    [SerializeField] AudioSource seAudioSource;
    [SerializeField] AudioSource bgmAudioSourceA;
    [SerializeField] AudioSource bgmAudioSourceB;

    float currentVolumeScale = 1;
    float nextVolumeScale = 1;

    AudioSource currentBGMAudioSource;
    AudioSource nextBGMAudioSource;

    bool crossFadeBGMToA;

    bool crossFading;
    float crossFadeTimer = 0;
    float crossFadeSeconds;

    //[Header("Debug")]
    //[SerializeField] bool debugAudio;
    //[SerializeField] AudioClip bgmA;
    //[SerializeField] AudioClip bgmB;
    //[SerializeField] AudioClip seA;

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
                nextBGMAudioSource.volume = nextVolumeScale;
            }
            else
            {
                currentBGMAudioSource.volume = currentVolumeScale * (1 - (crossFadeTimer / crossFadeSeconds));
                nextBGMAudioSource.volume = nextVolumeScale * (crossFadeTimer / crossFadeSeconds);
            }
        }

        //if (debugAudio && bgmA && bgmB)
        //{
        //    if (Input.GetKeyDown("a"))
        //    {
        //        MPlayBGMCrossFade(bgmA, 2.0f, 0.1f);
        //    }
        //    if (Input.GetKeyDown("d"))
        //    {
        //        MPlayBGMCrossFade(bgmB, 3.0f, 1.0f);
        //    }
        //    if (Input.GetKeyDown("s"))
        //    {
        //        PlayOneShot(seA, 5.0f);
        //    }
        //    if (Input.GetKeyDown("w"))
        //    {
        //        PlayOneShot(seA, 1.0f);
        //    }
        //}
    }

    AudioSource GetSEAudioSource()
    {
        return seAudioSource;
    }

    void MPlayBGMCrossFade(AudioClip _bgmClip, float _crossFadeSeconds, float _volumeScale)
    {
        if (_volumeScale > 1.0f)
        {
            Debug.LogError("velumeScaleは1.0f以上に対応していません");
        }

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
        nextVolumeScale = _volumeScale;
    }

    //*********************************************************
    static SimpleAudioManager sSimpleAudioManager;

    //**********************************************************
    //PlayOneShot
    public static void PlayOneShot(AudioClip _audioClip)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.GetSEAudioSource()?.PlayOneShot(_audioClip);
    }
    public static void PlayOneShot(AudioClip _audioClip, float _volumeScale)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.GetSEAudioSource()?.PlayOneShot(_audioClip, _volumeScale);
    }
    public static void PlayOneShot(SEAudioClip _seAudioClip)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.GetSEAudioSource()?.PlayOneShot(_seAudioClip.clip, _seAudioClip.volumeScale);
    }

    //**********************************************************
    //PlayBGMOverride
    public static void PlayBGMOverride(AudioClip _bgmClip)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.MPlayBGMCrossFade(_bgmClip, 0.0f, 1.0f);
    }
    public static void PlayBGMOverride(AudioClip _bgmClip, float _volumeScale)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.MPlayBGMCrossFade(_bgmClip, 0.0f, _volumeScale);
    }

    //**********************************************************
    //PlayBGMCrossFade
    public static void PlayBGMCrossFade(AudioClip _bgmClip, float _crossFadeSeconds)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.MPlayBGMCrossFade(_bgmClip, _crossFadeSeconds, 1.0f);
    }
    public static void PlayBGMCrossFade(AudioClip _bgmClip, float _crossFadeSeconds, float _volumeScale)
    {
        if (!sSimpleAudioManager)
        {
            Debug.LogError("SimpleAudioManagerが登録されていません");
            return;
        }

        sSimpleAudioManager.MPlayBGMCrossFade(_bgmClip, _crossFadeSeconds, _volumeScale);
    }

}

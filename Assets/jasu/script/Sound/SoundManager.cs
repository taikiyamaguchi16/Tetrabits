using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundParameter
{
    public AudioClip audioClip;

    public string name = "sound";

    public float volumeRate = 1f;

    SoundParameter()
    {
        Init();
    }

    public void Init()
    {
        volumeRate = 1f;
    }
}


[System.Serializable]
public class SoundEffectParameter
{
    public AudioSource audioSource = null;

    public SoundParameter soundParameter;
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    // BGM
    [SerializeField]
    AudioSource bgmAudioSource = null;

    static AudioSource sBgmAudioSource = null;

    [SerializeField]
    SoundParameter[] bgms;

    static Dictionary<string, SoundParameter> sBgmDic = new Dictionary<string, SoundParameter>();

    // SE
    [SerializeField]
    AudioSource seDefaultAudioSource = null;

    [SerializeField]
    SoundEffectParameter[] SoundEffects = null;

    static Dictionary<string, SoundEffectParameter> sSoundEffectDic = new Dictionary<string, SoundEffectParameter>();

    private void Awake()
    {
        // BGMのAudioSource初期化
        sBgmAudioSource = bgmAudioSource;

        if(sBgmAudioSource == null)
            sBgmAudioSource = GetComponent<AudioSource>();

        sBgmAudioSource.loop = true;

        // BGMリスト初期化
        if (sBgmDic.Count != 0)
            sBgmDic.Clear();

        foreach(var bgm in bgms)
        {
            sBgmDic.Add(bgm.name, bgm);
        }

        // SEの初期化
        if (seDefaultAudioSource == null)
            seDefaultAudioSource = GetComponent<AudioSource>();

        if (sSoundEffectDic.Count != 0)
            sSoundEffectDic.Clear();

        foreach (var se in SoundEffects)
        {
            sSoundEffectDic.Add(se.soundParameter.name, se);
        }
        
        foreach (var soundEffect in sSoundEffectDic)
        {
            if (soundEffect.Value.audioSource == null)
                soundEffect.Value.audioSource = seDefaultAudioSource;
        }
    }

    private void OnDestroy()
    {
        sBgmAudioSource.Stop();

        foreach (var se in sSoundEffectDic)
        {
            se.Value.audioSource.Stop();
        }
    }

    // BGM再生
    static public void PlayBGM(string _name)
    {
        //if (sBgmDic.ContainsKey(_name))
        //{
        //    sBgmAudioSource.clip = sBgmDic[_name].audioClip;
        //    sBgmAudioSource.volume *= sBgmDic[_name].volumeRate;
        //    sBgmAudioSource.Play();
        //    return;
        //}

        //Debug.Log("指定の名前のBGMが見つかりませんでした : " + _name);

        sBgmAudioSource.clip = sBgmDic[_name].audioClip;
        sBgmAudioSource.volume *= sBgmDic[_name].volumeRate;
        sBgmAudioSource.Play();
    }

    // BGM停止
    static public void StopBGM()
    {
        sBgmAudioSource.Stop();
        sBgmAudioSource.clip = null;
    }

    // 一時停止
    static public void PauseBGM()
    {
        sBgmAudioSource.Pause();
    }

    // 一時停止解除
    static public void UnPauseBGM()
    {
        sBgmAudioSource.UnPause();
    }

    // BGMミュート
    static public void MuteBGM()
    {
        sBgmAudioSource.mute = true;
    }

    // BGMミュート解除
    static public void UnMuteBGM()
    {
        sBgmAudioSource.mute = false;
    }

    // BGMのAudioSource取得
    static public AudioSource GetBgmAudioSource()
    {
        return sBgmAudioSource;
    }

    // SE再生
    static public void PlaySE(string _name)
    {
        sSoundEffectDic[_name].audioSource.PlayOneShot(sSoundEffectDic[_name].soundParameter.audioClip,
            sSoundEffectDic[_name].audioSource.volume * sSoundEffectDic[_name].soundParameter.volumeRate);
    }

    // SE停止
    static public void StopSE(string _name)
    {
        sSoundEffectDic[_name].audioSource.Stop();
    }

    // SEのAudioSource取得
    static public AudioSource GetSeAudioSource(string _name)
    {
        return sSoundEffectDic[_name].audioSource;
    }
}

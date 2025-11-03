using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<string, AudioClip> _sounds;
    public AudioMixer audioMixer;
    private AudioSource _bgmSoundSource;
    private AudioSource _sfxSoundSource;

    private string _currentBGMKey = null;

    public float masterVolume;
    public float bgmVolume;
    public float sfxVolume;

    protected override void Awake()
    {
        base.Awake();
        _sounds = new Dictionary<string, AudioClip>();

        _bgmSoundSource = transform.AddComponent<AudioSource>();
        _sfxSoundSource = transform.AddComponent<AudioSource>();

        audioMixer = Resources.Load<AudioMixer>("Sounds/MasterAudioMixer");

        _bgmSoundSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("BGM")[0];
        _sfxSoundSource.outputAudioMixerGroup = audioMixer.FindMatchingGroups("SFX")[0];

        var settingSave = DataManager.Instance.SaveData.SettingSave;
        // 저장된 불륨 불러오기, 기본값 1f
        masterVolume = settingSave.MasterVolume;
        bgmVolume = settingSave.BgmVolume;
        sfxVolume = settingSave.SoundEffectVolume;
        //
        SetMasterVolume(masterVolume);
        SetBGMVolume(bgmVolume);
        SetSfxVolume(sfxVolume);
    }

    // public void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         AchievementManager.Instance.AddParam("playedOverOneHr", 1);
    //     }
    //     if (Input.GetKeyDown(KeyCode.D))
    //     {
    //         AchievementManager.Instance.AddParam("winCount", 1);
    //     }
    //     if (Input.GetKeyDown(KeyCode.S))
    //     {
    //         AchievementManager.Instance.AddParam("criticalKill", 1);
    //     }
    //     if (Input.GetKeyDown(KeyCode.E))
    //     {
    //         AchievementManager.Instance.AddParam("receivedDamage", 1000);
    //     }
    // }
    
    public AudioClip LoadSound(string key)
    {
        var handle = Resources.Load<AudioClip>(key);
        if (_sounds.TryGetValue(key, out AudioClip clip))
        {
            //Debug.Log($"{key} is duplicate in sounds");
            return handle;
        }
        _sounds.Add(key, handle);
        return handle;
    }

    public void PlayBGM(string key, float volume = 1f) //volume = 각각의 사운드(하나) 불륨값 조절용
    {
        if (_currentBGMKey == key) return;

        _bgmSoundSource.clip = LoadSound(key);
        _bgmSoundSource.volume = volume;
        _bgmSoundSource.Play();

        _currentBGMKey = key;
    }

    public void PlaySfx(string key, float volume = 1f)
    {
        _sfxSoundSource.PlayOneShot(LoadSound(key), volume);
    }

    public AudioClip GetCurrentBGM()
    {
        return _bgmSoundSource?.clip;
    }

    public void SetMasterVolume(float volume) //bool save 이거 뮤트 설정하려고 따로 둔건데, 추가구현 안 할거면 안 쓰는 부분임.
    {
        masterVolume = volume;
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(masterVolume, 0.0001f, 1f)) * 20); //오디오믹서 값 변경
    }
    public void SetBGMVolume(float volume)
    {
        bgmVolume = volume;
        audioMixer.SetFloat("Bgm", Mathf.Log10(Mathf.Clamp(bgmVolume, 0.0001f, 1f)) * 20);
    }
    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        audioMixer.SetFloat("Sfx", Mathf.Log10(Mathf.Clamp(sfxVolume, 0.0001f, 1f)) * 20);
    }

    public void MuteAudio()
    {
        audioMixer.SetFloat("Master", Mathf.Log10(Mathf.Clamp(0, 0.0001f, 1f)) * 20); //오디오믹서 값 변경
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionMiddleSound : MonoBehaviour
{
    public Image muteButton;
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public bool isMute;
    public Sprite muteImage;
    public Sprite unmuteImage;
    public bool isInitialized;
    public bool isNotOverwrite = false; //초기값 불러올 때 슬라이더 값 덮어씌여지는 문제 방지용

    private void OnDisable()
    {
        if (!isInitialized) return;
        var masterVolume = AudioManager.Instance.masterVolume;
        var bgmVolume = AudioManager.Instance.bgmVolume;
        var sfxVolume = AudioManager.Instance.sfxVolume;
        
        DataManager.Instance.OnSettingsChanged(masterVolume,bgmVolume,sfxVolume, isMute);
    }

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        isNotOverwrite = true;
        
        masterSlider.value = AudioManager.Instance.masterVolume;
        bgmSlider.value = AudioManager.Instance.bgmVolume;
        sfxSlider.value = AudioManager.Instance.sfxVolume;
        isMute = DataManager.Instance.SaveData.SettingSave.IsMuted;
        if (isMute)
        {
            muteButton.sprite = muteImage;
        }
        isInitialized = true;
        isNotOverwrite = false;
    }

    public void OnValueChanged()
    {
        if (isNotOverwrite) return;
        AudioManager.Instance.SetMasterVolume(masterSlider.value);
        AudioManager.Instance.SetBGMVolume(bgmSlider.value);
        AudioManager.Instance.SetSfxVolume(sfxSlider.value);
        muteButton.sprite = unmuteImage;
        isMute = false;
        
        Debug.Log($"OnValueChanged {masterSlider.value}, {bgmSlider.value}, {sfxSlider.value}");
    }

    public void OnClickMute()
    {
        if (!isMute)
        { //뮤트 상태
            AudioManager.Instance.MuteAudio();
            isMute = true;
            muteButton.sprite = muteImage;
        }
        else
        { //뮤트x
            AudioManager.Instance.SetMasterVolume(masterSlider.value);
            isMute = false;
            muteButton.sprite = unmuteImage;
        }
        
    }
}

public class SettingSaveData
{
    public float MasterVolume { get; set; }
    public float BgmVolume { get; set; }
    public float SoundEffectVolume { get; set; }
    public bool IsMuted { get; set; }
    
    public SettingSaveData()
    {
        MasterVolume = 1.0f;
        BgmVolume = 1.0f;
        SoundEffectVolume = 1.0f;
        IsMuted = false;
    }
    public SettingSaveData(float masterVolume, float bgmVolume, float soundEffectVolume, bool isMuted)
    {
        MasterVolume = masterVolume;
        BgmVolume = bgmVolume;
        SoundEffectVolume = soundEffectVolume;
        IsMuted = isMuted;
    }
}

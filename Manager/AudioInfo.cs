using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioInfo : Singleton<AudioInfo>
{
    [Header("BGM")]
    public string titleBGM = "Sounds/TitleBGM2";
    public string corridorBGM = "Sounds/CorridorBGM";
    public string battleBGM = "Sounds/BattleBGM2";
    public string bossBGM = "Sounds/BossBGM2";
    public string gameOverBGM = "Sounds/GameOverBGM";
    
    [Header("SFX")]
    public string achievementSfx = "Sounds/AchievementSFX";
    public string shopBuySfx = "Sounds/ShopBuySFX";
    public string shopDenySfx = "Sounds/ShopDenySFX";
    public string chestOpenSfx = "Sounds/ChestOpenSFX";
    public string battleStartSfx = "Sounds/BattleStartSFX";
    public string attackSfx = "Sounds/AttackSFX";
    public string battleClearSfx = "Sounds/BattleClearSFX";
    public string stoneFallingSfx = "Sounds/StoneFallingSFX";
    
    [Header("BGMVolume")]
    public float titleBGMVolume = 0.5f;
    public float corridorBGMVolume = 0.5f;
    public float battleBGMVolume = 0.5f;
    public float bossBGMVolume = 0.5f;
    public float gameOverBGMVolume = 0.6f;
    
    [Header("SFXVolume")]
    public float achievementSfxVolume = 0.5f;
    public float shopBuySfxVolume = 1f;
    public float shopDenySfxVolume = 0.3f;
    public float chestOpenSfxVolume = 1f;
    public float battleStartSfxVolume = 0.8f;
    public float attackSfxVolume = 1f;
    public float battleClearSfxVolume = 1f;
    public float stoneFallingSfxVolume = 1f;
}

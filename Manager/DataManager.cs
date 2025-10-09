using DataTable;
using System;
using System.Collections;
using System.Collections.Generic;
using UGS;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public SkillDatas Skill;
    public MercenaryDatas Mercenary;//Player로 변경해도 됨.
    public EnemyDatas Enemy;
    public EffectDatas Effect;
    public WeaponDatas Weapon;
    public BattleDatas Battle;
    public ConsumableDatas Consumable;
    public ItemDatas Item;
    public GameDatas Game;
    public StoreDatas Store;
    public MapDatas Map;
    public RewardDatas Reward;

    public void Initialize()
    {
        UnityGoogleSheet.LoadAllData();
        Skill = new SkillDatas();
        Mercenary = new MercenaryDatas();
        Enemy = new EnemyDatas();
        Effect = new EffectDatas();
        Weapon = new WeaponDatas();
        Battle = new BattleDatas();
        Consumable = new ConsumableDatas();
        Map = new MapDatas();
        Reward = new RewardDatas();
        Item = new ItemDatas();
        Game = new GameDatas();
        Store = new StoreDatas();
    }
}
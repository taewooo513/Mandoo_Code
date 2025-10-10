using System.Collections;
using System.Collections.Generic;
using GoogleSheet.Core.Type;
using JetBrains.Annotations;
using UnityEngine;


[UGS(typeof(RoleType))]
public enum RoleType
{
    Nuker,
    Initiator,
    Lurker,
    Supporter
}
[UGS(typeof(EffectType))]
public enum EffectType
{
    Attack,
    Heal,
    Buff,
    Debuff,
    Mark,
    Protect
}
[UGS(typeof(TargetType))]
public enum TargetType
{
    Single,
    Range
}

[UGS(typeof(WeaponType))]
public enum WeaponType
{
    TwoHandedSword,
    Spear,
    Shield,
    Bow,
    ShortSword,
    Knuckle,
    SwordAndShield,
    Staff
}
[UGS(typeof(BuffType))]
public enum BuffType
{
    None,
    AttackUp,
    DefenseUp,
    SpeedUp,
    EvasionUp,
    CriticalUp,
    AllStatUp
}

[UGS(typeof(DeBuffType))]
public enum DeBuffType
{
    None,
    AttackDown,
    DefenseDown,
    SpeedDown,
    EvasionDown,
    CriticalDown,
    AllStatDown,
    Damaged,
    Stun
}

[UGS(typeof(ConsumableType))]
public enum ItemType
{
    None,
    Weapon,
    Consumable,
    Gold
}

[UGS(typeof(ConsumableType))]
public enum ConsumableType
{
    None,
    BattleSupport,
    ExplorerSupport,
    Gold,
    Map
}

[UGS(typeof(EventType))]
public enum EventType
{
    Battle,
    Treasure,
    Trap
}

public enum RoomDirection
{
    Up,
    Right,
    Down,
    Left
}

public enum RoomType
{
    Start,
    Empty,
    Battle,
    Item,
    Shop,
    PMC,
    Village
}
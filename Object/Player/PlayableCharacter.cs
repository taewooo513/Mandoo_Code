using DataTable;
using DefaultTable;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayableCharacter : BaseEntity
{
    [SerializeField] private int initID;
    private MercenaryData data;
    private List<int> _deadEquipItemIds = new();
    private void Start()
    {
        characterAnimationController = GetComponentInChildren<PlayableCharacterAnimationController>();
        //Init(initID);
    }

    public override void Init(int id)
    {
        SetData(id);
        buffIcons.UpdateIcon(entityInfo.statEffect);
    }

    public void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical, data.gameObjectString, data.roleType
        );
        entityInfo.SetUpSkill(data.skillId, this);

        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills);
    }

    public override void Attack(float dmg, BaseEntity baseEntity)
    {
        base.Attack(dmg, baseEntity);
        BattleManager.Instance.AttackEntity(
            Utillity.GetIndexInListToObject(BattleManager.Instance._enemyCharacters, baseEntity), dmg);
    }

    public override void UseSkill(Action action, BaseEntity baseEntity)
    {
        characterAnimationController.Attack(action, baseEntity);
    }

    public override void UseSkill(Action action, List<BaseEntity> baseEntitys)
    {
        characterAnimationController.Attack(action, baseEntitys);
    }
    public override void StartExtraTurn()
    {
        BattleManager.Instance.isEndTrun = true;
        entityInfo.statEffect.AttackWeight(entityInfo);
        Debug.Log("응가");
        if (entityInfo.IsStun())
        {
            BattleManager.Instance.EndTurn(false);
            return;
        }
    }
    public override void StartTurn()
    {
        base.StartTurn();
        BattleManager.Instance.isEndTrun = true;
        entityInfo.statEffect.AttackWeight(entityInfo);
        if (entityInfo.IsStun())
        {
            BattleManager.Instance.EndTurn(false);
            return;
        }
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills);
    }

    public void UpdateUI()
    {
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills);
    }

    public override void EndTurn()
    {
        List<BuffType> buffTypes = new List<BuffType>();
        List<DeBuffType> deBuffTypes = new List<DeBuffType>();
        buffTypes.Add(BuffType.AttackUp);
        buffTypes.Add(BuffType.DefenseUp);
        buffTypes.Add(BuffType.SpeedUp);
        buffTypes.Add(BuffType.EvasionUp);
        buffTypes.Add(BuffType.CriticalUp);
        buffTypes.Add(BuffType.AllStatUp);

        deBuffTypes.Add(DeBuffType.AttackDown);
        deBuffTypes.Add(DeBuffType.DefenseDown);
        deBuffTypes.Add(DeBuffType.SpeedDown);
        deBuffTypes.Add(DeBuffType.EvasionDown);
        deBuffTypes.Add(DeBuffType.CriticalDown);
        deBuffTypes.Add(DeBuffType.AllStatDown);
        deBuffTypes.Add(DeBuffType.Damaged);
        deBuffTypes.Add(DeBuffType.Stun);

        entityInfo.statEffect.ReduceTurn(buffTypes, deBuffTypes);
        Damaged(entityInfo.statEffect.totalStat.damagedValue);
        buffIcons.UpdateIcon(entityInfo.statEffect);


    }

    public override void Damaged(float value)
    {
        int lastHp = entityInfo.currentHp;
        base.Damaged(value);
        if (entityInfo.currentHp != lastHp && !entityInfo.isDie)
        {
            characterAnimationController.Damaged();
        }
        else if (entityInfo.isDie)
        {
            characterAnimationController.Die();
        }
    }

    //public void EquipWeapon(Weapon weapon)
    //{
    //    if (IsEquipWeapon(weapon))
    //    {
    //        UnEquipWeapon();
    //        return;
    //    }
    //    entityInfo.equipWeapon = weapon;
    //    entityInfo.skills[3] = weapon.skill;
    //}
    //private void UnEquipWeapon()
    //{
    //    entityInfo.equipWeapon = null;
    //    entityInfo.skills[3] = null;
    //}
    //private bool IsEquipWeapon(Weapon weapon)
    //{
    //    if (entityInfo.equipWeapon == weapon)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    // TODO: 장착/획득 아이템 전분 인벤토리매니저에 넘겨주기.
}
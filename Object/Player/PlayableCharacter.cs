using DataTable;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayableCharacter : BaseEntity
{
    [SerializeField] private int initID;
    private MercenaryData data;
    public Image myTurnCheckImg;
    private Color _defaultColor;
    private Color _turnColor;

    private void Start()
    {
        characterAnimationController = GetComponentInChildren<PlayableCharacterAnimationController>();
        //Init(initID);
        _defaultColor = new Color(120f / 255f, 120 / 255f, 120f / 255f);
        _turnColor = new Color(255f / 255f, 255f / 255f, 0f / 255f);
    }

    public override void Init(int id)
    {
        base.Init(id);
        SetData(id);
        buffIcons.UpdateIcon(entityInfo.statEffect);
    }

    public void SetData(int id)
    {
        this.id = id;
        data = DataManager.Instance.Mercenary.GetMercenaryData(id);

        entityInfo = new EntityInfo(
            data.name, data.health, data.attack, data.defense, data.speed, data.evasion, data.critical, data.gameObjectPrefabString, data.roleType, data.gameObjectString
        );
        entityInfo.SetUpSkill(data.skillId, this);

        AssetSetting();
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
    }

    public override void AssetSetting()
    {
        if (id != 0)
        {
            string mercenaryPrefabPath = DataManager.Instance.Mercenary.GetMercenaryData(id).gameObjectPrefabString;
            GameObject modelPrefab = Resources.Load<GameObject>(mercenaryPrefabPath);
            Instantiate(modelPrefab, transform);
        }
    }

    public override void Attack(float dmg, BaseEntity baseEntity)
    {
        base.Attack(dmg, baseEntity);
        BattleManager.Instance.AttackEntity(
            Utillity.GetIndexInListToObject(BattleManager.Instance._enemyCharacters, baseEntity), dmg);
    }

    public override void UseSkill(Action action, BaseEntity baseEntity, Skill skill)
    {
        characterAnimationController.Attack(action, baseEntity, skill);
    }

    public override void UseSkill(Action action, List<BaseEntity> baseEntitys, Skill skill)
    {
        characterAnimationController.Attack(action, baseEntitys, skill);
    }
    public override void StartExtraTurn()
    {
        extraTurnText.OnExtraTurn();
        myTurnCheckImg.color = _turnColor;
        BattleManager.Instance.isEndTurn = true;
        entityInfo.statEffect.AttackWeight(entityInfo);
        if (entityInfo.IsStun())
        {
            BattleManager.Instance.EndTurn(false);
            return;
        }
    }
    public override void StartTurn()
    {
        base.StartTurn();
        BattleManager.Instance.isUseItem = false;
        myTurnCheckImg.color = _turnColor;
        entityInfo.statEffect.AttackWeight(entityInfo);
        if (entityInfo.IsStun())
        {
            BattleManager.Instance.EndTurn(false);
            return;
        }
        BattleManager.Instance.isEndTurn = true;

        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
    }

    public void UpdateUI()
    {
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
    }

    public override void EndTurn()
    {
        myTurnCheckImg.color = _defaultColor;
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
        AchievementManager.Instance.AddParam("receivedDamage", (int)value);

        if (entityInfo.currentHp != lastHp && !entityInfo.isDie)
        {
            characterAnimationController.Damaged();
        }
        else if (entityInfo.isDie)
        {

            characterAnimationController.Die();
        }
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI();
    }

    public InGameItem EquipWeapon(Weapon weapon)
    {
        InGameItem res = null;
        if (entityInfo.equips[0] == null)
        {
            entityInfo.equips[0] = weapon;
            res = null;
        }
        else
        {
            (entityInfo.equips[0], weapon) = (weapon, entityInfo.equips[0]);
            res = weapon;
        }
        Skill weaponSkill = new Skill();
        weaponSkill.Init(weapon.skillId, this);
        entityInfo.skills[3] = weaponSkill;
        entityInfo.GetTotalBuffStat().Reset(entityInfo);
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
        //UIManager.Instance.OpenUI<WeaponTutorial>();
        if (GameManager.Instance.CurrentMapIndex == 0)
            AnalyticsManager.Instance.SendEventStep(7);
        Tutorials.ShowIfNeeded<WeaponTutorial>();
        UIManager.Instance.CloseUI<StopWPTutorial>();
        return res;
    }
    public Weapon UnEquipWeapon()
    {
        Weapon weapon = entityInfo.equips[0];
        if (entityInfo.equips[0] != null)
        {
            entityInfo.equips[0] = null;
        }
        entityInfo.GetTotalBuffStat().Reset(entityInfo);
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
        return weapon;
    }
    public InGameItem EquipAcc(Weapon weapon, int index)
    {
        if (entityInfo.equips[index] == null)
        {
            entityInfo.equips[index] = weapon;
            entityInfo.GetTotalBuffStat().Reset(entityInfo);
            UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
            return null;
        }
        else
        {
            (entityInfo.equips[index], weapon) = (weapon, entityInfo.equips[index]);
            entityInfo.GetTotalBuffStat().Reset(entityInfo);
            UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
            return weapon;
        }
    }

    public void UnEquipAcc(int index)
    {
        entityInfo.equips[index] = null;
        entityInfo.GetTotalBuffStat().Reset(entityInfo);
        UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(entityInfo, entityInfo.skills, this);
    }

    public bool IsEquipment(int index)
    {
        if (entityInfo.equips[index] != null)
        {
            return true;
        }
        return false;
    }
    // TODO: 장착/획득 아이템 전분 인벤토리매니저에 넘겨주기.
}
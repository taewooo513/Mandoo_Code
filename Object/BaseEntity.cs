using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityInfo
{
    public string name;
    public int currentHp;
    public int maxHp;
    public int attackDamage;
    public int defense;
    public int speed;
    public RoleType role;
    public bool isDie;
    public float evasion;
    public float critical;
    public Buff statEffect;
    public float hpWeight = 0f;
    public float addWeight = 0.3f;
    public Skill[] skills;
    public string iconPath;

    private readonly float _standardWeight = 0.25f;
    public bool canMove = true;
    public int contractGold;
    public string gameObjectString;

    public TotalBuffStat GetTotalBuffStat()
    {
        statEffect.AttackWeight(this);
        return statEffect.totalStat;
    }

    public EntityInfo(string name, int maxHp, int attackDamage, int defense, int speed, float evasion, float critical, string iconPath, RoleType role)
    {
        this.name = name;
        this.maxHp = maxHp;
        this.currentHp = maxHp;
        this.attackDamage = attackDamage;
        this.defense = defense;
        this.speed = speed;
        this.evasion = evasion;
        this.critical = critical;
        statEffect = new Buff();
        this.gameObjectString = iconPath;
        this.role = role;
    }

    public void Damaged(float value)
    {
        var hp = currentHp - value;
        currentHp = (int)hp;
        if (currentHp <= 0)
        {
            currentHp = 0;
            isDie = true;
        }
    }

    public float GetPlayableTargetWeight() //플레이어블 캐릭터의 타깃 가중치 합
    {
        float result = _standardWeight + statEffect.totalStat.totalWeight; //가중치 합
        GenerateWeightListUtility.CombineWeights(result); //가중치를 리스트에 추가 //TODO : 턴 끝날 때 GenerateWeightListUtility.Clear(); 호출해줘야 됨
        return result;
    }

    public float GetEnemyTargetWeight() //enemy의 타깃 가중치 합
    {
        float result = _standardWeight;
        GenerateWeightListUtility.CombineWeights(result);
        return result;
    }

    //TODO : 스킬 확률 관리 부분에서, 스킬 효과에 따른 증감 가중치 주려면 구조 변경해야됨. curHP를 넘겨서 따로 작업?
    public bool LowHPStatEnemy() //적(플레이어블) hp가 낮을 때.
    {
        double percentage = maxHp * 0.4;
        if (currentHp <= percentage) //현재 hp가 40% 이하일 때
        {
            return true;
        }
        return false;
    }

    public bool LowHPStatPlayer() //아군(enemy) hp가 낮을 때.
    {
        double percentage = maxHp * 0.1;
        if (currentHp <= percentage) //현재 hp가 10% 이일 때
        {
            return true;
        }
        return false;
    }

    public void SetUpSkill(List<int> skillIdList, BaseEntity nowEntity)
    {
        skills = new Skill[skillIdList.Count];
        for (int i = 0; i < skillIdList.Count; i++)
        {
            skills[i] = new Skill();
            skills[i].Init(skillIdList[i], nowEntity);
        }
    }
    public void Heal(float value)
    {
        var hp = currentHp + value;
        currentHp = (int)hp;
        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }
    public void AddEffect(BuffInfo statEffectInfo)
    {
        statEffect.AddStatus(statEffectInfo);
    }

    public bool IsStun()
    {
        return statEffect.IsStun();
    }
}

public class BaseEntity : MonoBehaviour
{
    [SerializeField] public EntityInfo entityInfo;
    protected bool isNowExtraTurn = false;
    public EntityInfo GetEntityInfo
    {
        get { return entityInfo; }
    }
    private HpbarUI hpbarUI;
    public int id { get; protected set; }
    protected bool hasExtraTurn = true;
    public Action<BaseEntity> OnDied;
    protected BuffIcons buffIcons;
    public EntityCharacterAnimationController characterAnimationController;
    protected virtual void Awake()
    {
        SetData();
        hpbarUI = GetComponentInChildren<HpbarUI>();
        buffIcons = GetComponentInChildren<BuffIcons>();
    }

    public virtual void Init(int id)
    {

    }

    public virtual void Release()
    {
        OnDied -= BattleManager.Instance.EntityDead;
    }

    public virtual void SetData()
    {
    }

    public virtual void Damaged(float value)
    {
        entityInfo.Damaged(value);
        hpbarUI.UpdateUI();
    }

    public void BattleStarted()
    {
        OnDied += BattleManager.Instance.EntityDead;
    }

    public virtual void Attack(float dmg, BaseEntity baseEntity)
    {

    }

    public virtual void Support(Skill skill, BaseEntity baseEntity)
    {

    }
    public virtual void UseSkill(Action action, BaseEntity baseEntity)
    {

    }

    public virtual void UseSkill(Action action, List<BaseEntity> baseEntitys)
    {

    }
    public void AddEffect(BuffInfo statEffectInfo)
    {
        entityInfo.AddEffect(statEffectInfo);
        buffIcons.UpdateIcon(entityInfo.statEffect);
    }

    public virtual void Heal(float value)
    {
        entityInfo.Heal(value);
        hpbarUI.UpdateUI();
    }
    public virtual void StartTurn()
    {
    }
    public virtual void EndTurn()
    {

    }
    public virtual void EndExtraTurn()
    {

    }
    public virtual void StartExtraTurn()
    {

    }
}
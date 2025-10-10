using UnityEngine;
using System.Collections.Generic;
using DataTable;
using System.Linq;
using UnityEditorInternal;
using System;

public class Enemy : BaseEntity
{
    private EnemyData _data;
    private Skill[] _skills;
    private bool _hasExtraTurn = true;
    private Skill _attackSkill;
    [SerializeField] private int initID;
    private void Start()
    {
        characterAnimationController = GetComponentInChildren<EnemyCharacterAnimationController>();
        //Init(initID);
    }
    public override void Init(int idx)
    {
        SetData(idx);
        buffIcons.UpdateIcon(entityInfo.statEffect);
    }

    private void SetData(int idx)
    {
        this.id = idx;
        _data = DataManager.Instance.Enemy.GetEnemyData(idx);
        entityInfo = new EntityInfo(
            _data.name, _data.health, _data.attack, _data.defense, _data.speed, _data.evasion, _data.critical, _data.gameObjectString, _data.roleType
        );
        entityInfo.SetUpSkill(_data.skillId, this);
    }

    public override void StartTurn()
    {
        base.StartTurn();
        entityInfo.statEffect.AttackWeight(entityInfo);
        if (entityInfo.IsStun())
        {
            BattleManager.Instance.EndTurn(false);
            return;
        }

        EnemyAction();
    }

    public void EnemyAction()
    {
        bool isAttack = false; //공격 타입의 스킬공격인지
        _attackSkill = GetRandomSkill();
        if (_attackSkill == null) //선택한 스킬이 null이면
        {
            BattleManager.Instance.EndTurn(false);
            return;
        }

        foreach (var item in _attackSkill.skillInfo.skillEffects) //스킬효과를 돌면서
        {
            var effectType = item.GetEffectType(); //스킬 효과 타입 챙겨오기
            if (effectType == EffectType.Attack || effectType == EffectType.Debuff) //공격류 스킬이면
            {
                isAttack = true;
                break;
            }
        }
        if (isAttack) //공격류 스킬일 시 실행
        {
            if (TryAttack(out int position)) //공격 시도 성공 시
            {
                _hasExtraTurn = true; //추가 공격 체크용
            }
            else //공격 시도 실패 시
            {
                _hasExtraTurn = false;
                if (position != -1)
                {
                    BattleManager.Instance.SwitchPosition(this, position); //이동
                    BattleManager.Instance.EndTurn();
                }
                else
                {
                    BattleManager.Instance.EndTurn();
                }
            }
        }
        else //서포터류 스킬일 시 실행
        {
            if (TrySupport(out int position))
            {
                _hasExtraTurn = true;
            }
            else
            {
                _hasExtraTurn = false;
                if (position != -1)
                {
                    BattleManager.Instance.SwitchPosition(this, position); //이동
                    BattleManager.Instance.EndTurn();
                }
                else
                {
                    BattleManager.Instance.EndTurn();
                }
            }
        }
    }

    public override void EndTurn()
    {
        //TODO : 지금 엔티티에 걸린 상태이상을 적용하고, 턴 수를 감소시킨다?
        _attackSkill = null; //선택한 스킬 비워주기

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

    public override void StartExtraTurn() //추가 공격 턴
    {
        isNowExtraTurn = true;
        if (TryAttack(out int position)) //공격 시도 성공시
        {
        }
        else //공격 실패 시
        {
            if (position != -1)
            {
                BattleManager.Instance.SwitchPosition(this, position); //이동
                BattleManager.Instance.EndTurn();
            }
        }
    }

    private Skill GetRandomSkill()
    {
        var skillCandidates = new List<Skill>();
        //var weights = new List<float>();
        float weight = Skill.defaultWeight;
        if (entityInfo.skills == null || entityInfo.skills.Length == 0) return null;
        BattleManager.Instance.GetLowHpSkillWeight(out float playerWeight, out float enemyWeight);
        //TODO : 범위 공격 - 스킬 랜덤으로 뽑아주는 부분에서, 랜덤으로뽑힌스킬.UseSkill 하면 된다고 함.
        for (int i = 0; i < entityInfo.skills.Length; i++)
        {
            if (!CanUseSkill(entityInfo.skills[i]))
            {
                continue;
            }

            //TODO : 2차 때 캐릭터성/1회성 계산 로직
            var skill = entityInfo.skills[i];
            if (skill == null || skill.skillInfo == null) continue;

            var info = skill.skillInfo;
            if (info.enablePos == null || info.targetPos == null) continue;

            bool isAttackSkill = false;
            bool isSupportSkill = false;
            var effectArray = info.skillEffects;

            for (int j = 0; j < effectArray.Length; j++)
            {
                var effect = effectArray[j];
                var effectType = effect.GetEffectType();
                if (effectType == EffectType.Attack)
                    isAttackSkill = true;
                if (effectType == EffectType.Heal)
                    isSupportSkill = true;
            }

            if (isAttackSkill)
                skill.addedWeight = playerWeight + weight;
            if (isSupportSkill)
                skill.addedWeight = enemyWeight + weight;

            skillCandidates.Add(skill);
        }

        if (skillCandidates.Count == 0) return null;
        return RandomizeUtility.GetRandomSkillByWeight(skillCandidates);
    }

    private bool CanUseSkill(Skill skill) //스킬이 사용 가능한 위치 enemy가 서있는지, 스킬 범위 내에 플레이어가 서 있는지
    {
        if (skill == null || skill.skillInfo == null) return false;
        var info = skill.skillInfo;
        bool atEnablePosition = BattleManager.Instance.IsEnablePos(this, info.enablePos);
        bool atTargetPosition = BattleManager.Instance.IsTargetInList(info.targetPos);
        if (atEnablePosition && atTargetPosition)
            return true;
        return false;
    }

    public override void UseSkill(Action action, BaseEntity baseEntity)
    {
        base.UseSkill(action, baseEntity);
        characterAnimationController.Attack(action, baseEntity);
    }
    public override void UseSkill(Action action, List<BaseEntity> baseEntitys)
    {
        base.UseSkill(action, baseEntitys);
        characterAnimationController.Attack(action, baseEntitys);
    }

    private bool TryAttack(out int position) //스킬 선택, 타겟 선택
    {
        var info = _attackSkill.skillInfo; //사용할 스킬 정보
        List<int> targetRange =
            BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>(), BattleManager.Instance.PlayableCharacters[0]); //타겟 가능한 범위 가져오기
        List<float> weights = BattleManager.Instance.GetWeightList(true); //타겟 가중치 리스트 가져옴
        int pickedIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(weights); //가중치 기반으로 랜덤하게 플레이어 인덱스를 선택

        var targetEntity = BattleManager.Instance.PlayableCharacters[pickedIndex]; //타겟
        //TargetCheckUI(targetEntity);
        if (CanUseSkill(_attackSkill))
        {

            if (targetRange.Contains(pickedIndex)) //선택한 인덱스(때리려는 적)가 타겟 가능한 위치에 있는지 체크
            {
                _attackSkill
                    .UseSkill(targetEntity); //기존 : Attack(dmg, targetEntity); //스킬 작동 흐름 : tryAttack -> UseSkill -> Attack 순서
                position = -1;
                return true;
            }
        }

        position = GetDesiredPosition(_attackSkill); //현재 enemy가 서 있는 위치
        return false;
    }

    private bool TrySupport(out int position)
    {
        var info = _attackSkill.skillInfo; //사용할 스킬 정보
        List<int> targetRange =
            BattleManager.Instance.GetPossibleSkillRange(info.targetPos ?? new List<int>(), BattleManager.Instance.EnemyCharacters[0]); //타겟 가능한 범위 가져오기
        List<float> weights = BattleManager.Instance.GetWeightList(false); //타겟 가중치 리스트 가져옴
        int pickedIndex = RandomizeUtility.TryGetRandomPlayerIndexByWeight(weights); //가중치 기반으로 랜덤하게 플레이어 인덱스를 선택
        BaseEntity targetEntity;
        //teawoong
        targetEntity = BattleManager.Instance.EnemyCharacters[pickedIndex]; //타겟


        //TargetCheckUI(targetEntity);

        if (CanUseSkill(_attackSkill))
        {
            if (targetRange.Contains(pickedIndex)) //선택한 인덱스(때리려는 적)가 타겟 가능한 위치에 있는지 체크
            {
                _attackSkill
                    .UseSkill(targetEntity); //기존 : Attack(dmg, targetEntity); //스킬 작동 흐름 : tryAttack -> UseSkill -> Support 순서
                position = -1;
                return true;
            }
        }

        position = GetDesiredPosition(_attackSkill); //현재 enemy가 서 있는 위치
        return false;
    }

    public override void Attack(float dmg, BaseEntity targetEntity) //적->플레이어 공격
    {
        int index = Utillity.GetIndexInListToObject(BattleManager.Instance.PlayableCharacters, targetEntity);
        BattleManager.Instance.AttackEntity(index, (int)dmg); //범위/단일 공격 처리는 skill에서 되어있음
    }

    public override void Support(Skill skill, BaseEntity baseEntity) // 스킬 받아오는 구조로 변경
    {
        //아마 다른 서포터류 함수 만들고 그거 추가해주는 작업 할듯
    }

    // private void TargetCheckUI(BaseEntity targetEntity) //때리려는 타겟만 붉은색으로 표시해주는 UI 함수
    // {
    //     //플레이어 프리팹 접근해서 AttackChoice 켜주는 함수. 근데 이거 ui에서 해줘야됨.
    // }

    private int GetDesiredPosition(Skill skill) //스킬을 사용하기 위해 이동해야 할 위치를 반환하는 함수
    {
        if (skill == null || skill.skillInfo == null) return -1; //스킬이나 스킬 정보가 없으면 -1 반환
        var info = skill.skillInfo;
        if (info.enablePos == null || info.enablePos.Count == 0) return -1; //스킬 사용 가능 위치가 없으면 -1 반환

        var currentIndex = BattleManager.Instance.FindEntityPosition(this) ?? -1; //현재 엔티티의 위치 인덱스
        if (currentIndex < 0) return -1; //현재 위치를 찾을 수 없으면 -1 반환

        var entities = BattleManager.Instance.EnemyCharacters; //적 캐릭터 리스트
        int entityCount = entities?.Count ?? 0; //적 캐릭터 수 
        if (entityCount == 0) return -1; //적이 없으면 -1 반환

        foreach (var position in info.enablePos) //스킬 사용 가능 위치들을 순회
        {
            int targetIndex = position;
            // 타겟이 dead 상태일 경우 continue;
            // var target = BattleManager.Instance.PlayableCharacters[targetIndex];
            // if (BattleManager.Instance.EntityDead(target)) continue;

            if (targetIndex >= 0 && targetIndex < entityCount) //유효한 인덱스 범위인지 확인
            {
                if (targetIndex != currentIndex) //현재 위치와 다른 위치면 그 위치로 이동
                    return targetIndex;
            }

            return -1; //현재 위치와 같거나 유효하지 않은 위치면 그대로 반환
        }

        return -1; //적절한 위치를 찾지 못하면 -1 반환
    }
}
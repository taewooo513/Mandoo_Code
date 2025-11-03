using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSkill : SkillEffect
{
    public void DeBuffDamageSkill(int duration, BaseEntity targetEntity)
    {
    }

    // 캐릭터 데미지 적용
    public void HitDamageSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        float finalAtk = attackEntity.entityInfo.GetTotalBuffStat().attackDmg - damagedEntity.entityInfo.GetTotalBuffStat().defense;
        float dmg = Mathf.Max(finalAtk * adRatio + constantValue, 1);

        int temp = (int)(attackEntity.entityInfo.critical * 100f);

        if (Random.Range(0, 100) <= temp)
        {
            dmg *= 2;

        }
        int temp2 = (int)(damagedEntity.entityInfo.evasion * 100f);

        if (Random.Range(0, 100) <= temp2)
        {
            damagedEntity.evasionText.OnEvasion();
        }
        else
        {
            BattleManager.Instance.HitEnemyCount++;
            attackEntity.Attack(dmg, damagedEntity);
            if (damagedEntity.entityInfo.currentHp - dmg <= 0)
            {
                if (damagedEntity.entityInfo.role == RoleType.Boss) //보스 잡으면 무기 xp 지급
                {
                    var gameDataExplorerExp = GameManager.Instance.gameDatas.GetGameData(GameManager.Instance.CurrentMapIndex).bossExp;
                    if(attackEntity != null && attackEntity.entityInfo.equips[0] != null)
                        attackEntity.entityInfo.equips[0].AddWeaponExp(gameDataExplorerExp);
                }
                else //적이 보스가 아닐 때
                {
                    var gameDataExplorerExp = GameManager.Instance.gameDatas.GetGameData(GameManager.Instance.CurrentMapIndex).normalExp;
                    if(attackEntity != null && attackEntity.entityInfo.equips[0] != null)
                        attackEntity.entityInfo.equips[0].AddWeaponExp(gameDataExplorerExp);
                }

                if (dmg > Mathf.Max(finalAtk * adRatio + constantValue, 1))
                {
                    if (attackEntity.GetType() == typeof(PlayableCharacter))
                        AchievementManager.Instance.AddParam("criticalKill", 1);
                }
            }
        }
    }

    //아이템 데미지 적용(고정 수치만 적용, 착용자의 공격력 고려 X)
    public void HitDamageSkill(BaseEntity damagedEntity)
    {
        float dmg = constantValue;
        damagedEntity.Damaged(dmg);
        AchievementManager.Instance.AddParam("useBomb", 1);
        BattleManager.Instance.OnceUseItem();

    }

    public void UseBuffSkill(BaseEntity attackEntity, BaseEntity damagedEntity)
    {
        if (duration != 0)
        {
            DeBuffDamageSkill(duration, damagedEntity);
        }
        else
        {
            HitDamageSkill(attackEntity, damagedEntity);
        }
    }

    public override void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
        HitDamageSkill(actionEntity, targetEntity);
    }
    public override void ActiveEffect(BaseEntity targetEntity)
    {
        HitDamageSkill(targetEntity);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyCharacterAnimationController : EntityCharacterAnimationController
{
    int layers;
    BaseEntity targetEntity;
    List<BaseEntity> baseEntitys;
    SpriteRenderer sprites;
    BaseEntity nowEntity;
    protected override void Awake()
    {
        base.Awake();
        sprites = GetComponent<SpriteRenderer>();
        nowEntity = GetComponentInParent<Enemy>();
    }
    public override void Damaged()
    {
        animator.SetTrigger("Hit");
    }

    public override void Die()
    {
        animator.SetTrigger("Die");
    }

    public override void DieEvent()
    {
        foreach (InGameItem equipWeapon in nowEntity.entityInfo.equips)
        {
            GameManager.Instance.deadEquipWeapons.Add(equipWeapon);
        }

        nowEntity?.OnDied?.Invoke(nowEntity);
    }
    public override void Attack(Action action, BaseEntity targetEntity, Skill skill)
    {
        animator.SetTrigger("Attack");
        this.targetEntity = targetEntity;
        targetEntity.characterAnimationController.LayerUp();
        LayerUp();
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.attackSfx, AudioInfo.Instance.attackSfxVolume);
        this.action = action;
    }
    public override void Attack(Action action, List<BaseEntity> baseEntitys, Skill skill)
    {
        animator.SetTrigger("Attack");
        this.baseEntitys = baseEntitys;
        LayerUp();
        baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerUp(); });
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.attackSfx, AudioInfo.Instance.attackSfxVolume);
        this.action = action;
    }
    public override void LayerUp()
    {
        layers = sprites.sortingOrder;
        sprites.sortingOrder += 50;
        if(BattleManager.Instance.blackOutImage != null)
            BattleManager.Instance.blackOutImage.SetActive(true);
    }

    public override void LayerDown()
    {
        sprites.sortingOrder = layers;
        if(BattleManager.Instance.blackOutImage != null)
            BattleManager.Instance.blackOutImage.SetActive(false);
    }

    public override void ActionEvent()
    {
        action.Invoke();
        
    }
    public override void ActionEndEvent()
    {
        base.ActionEndEvent();
        LayerDown();

        if (targetEntity != null && targetEntity.characterAnimationController != null)
        {
            targetEntity.characterAnimationController.LayerDown();
            targetEntity = null;
        }
        else
        {
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
            baseEntitys = null;
        }
        BattleManager.Instance.EndTurn(true);
    }
}

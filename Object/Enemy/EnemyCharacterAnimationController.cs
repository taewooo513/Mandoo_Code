using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterAnimationController : EntityCharacterAnimationController
{
    int layers;
    BaseEntity targetEntity;
    List<BaseEntity> baseEntitys;
    SpriteRenderer sprites;
    BaseEntity nowEntity;
    public void Awake()
    {
        sprites = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        nowEntity?.OnDied?.Invoke(nowEntity);
    }
    public override void Attack(Action action, BaseEntity targetEntity)
    {
        animator.SetTrigger("Attack");
        this.targetEntity = targetEntity;
        targetEntity.characterAnimationController.LayerUp();
        LayerUp();
        AudioManager.Instance.PlaySound("Sounds/Attack");
        this.action = action;
    }
    public override void Attack(Action action, List<BaseEntity> baseEntitys)
    {
        animator.SetTrigger("Attack");
        this.baseEntitys = baseEntitys;
        LayerUp();
        baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerUp(); });
        AudioManager.Instance.PlaySound("Sounds/Attack");
        this.action = action;
    }
    public override void LayerUp()
    {
        layers = sprites.sortingOrder;
        sprites.sortingOrder += 50;
        BattleManager.Instance.blackOutImage.SetActive(true);
    }

    public override void LayerDown()
    {
        sprites.sortingOrder = layers;
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

        if (targetEntity != null)
        {
            targetEntity.characterAnimationController.LayerDown();
            targetEntity = null;
        }
        else
        {
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
            baseEntitys = null;
        }
        BattleManager.Instance.EndTurn();
    }
}

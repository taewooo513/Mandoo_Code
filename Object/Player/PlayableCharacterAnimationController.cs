using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayableCharacterAnimationController : EntityCharacterAnimationController
{
    int[] layers;
    BaseEntity targetEntity;
    List<BaseEntity> baseEntitys;
    SpriteRenderer[] sprites;
    BaseEntity nowEntity;
    protected override void Awake()
    {
        animator = GetComponent<Animator>();
        sprites = GetComponentsInChildren<SpriteRenderer>();
        nowEntity = GetComponentInParent<PlayableCharacter>();
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

    public override void LayerUp()
    {
        layers = new int[sprites.Length];
        for (int i = 0; i < sprites.Length; i++)
        {
            layers[i] = sprites[i].sortingOrder;
            sprites[i].sortingOrder += 50;
        }
        BattleManager.Instance.blackOutImage.SetActive(true);
    }

    public override void LayerDown()
    {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < sprites.Length; i++)
        {
            sprites[i].sortingOrder = layers[i];
        }
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
        }
        else
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
        BattleManager.Instance.EndTurn(false);
    }
}

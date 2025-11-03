using System;
using System.Collections.Generic;
using UnityEngine;

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

    public override void SetTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }

    public override void FlipX(bool isFlip)
    {
        if (isFlip)
        {
            transform.localScale = new Vector3(-1.5f, 1.5f, 1);
        }
        else
        {
            transform.localScale = new Vector3(1.5f, 1.5f, 1);
        }
    }

    public override void Attack(Action action, BaseEntity targetEntity, Skill skill)
    {
        animator.SetTrigger("Attack");
        this.targetEntity = targetEntity;
        targetEntity.characterAnimationController.LayerUp();
        LayerUp();
        var path = skill.skillInfo.effectPath;
        var type = skill.skillInfo.skillEffectType;
        var dir = new Vector3(targetEntity.transform.position.x - nowEntity.transform.position.x, 0, 0);
        // TODO: 데이터 테이블에 스킬 이펙트가 투사체인지 확인 후
        if (type == SkillEffectType.Hit)
            EffectManager.Instance.CreateEffect(path, targetEntity.transform.position); // 즉발 피격 이펙트 생성  
        else
            EffectManager.Instance.CreateEffect(path, nowEntity.transform.position, targetEntity.transform.position, dir); // 투사체나 캐스팅 이펙트 생성  
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.attackSfx, AudioInfo.Instance.attackSfxVolume);
        this.action = action;
    }
    public override void Attack(Action action, List<BaseEntity> baseEntitys, Skill skill)
    {
        animator.SetTrigger("Attack");
        this.baseEntitys = baseEntitys;
        LayerUp();
        var path = skill.skillInfo.effectPath;
        var type = skill.skillInfo.skillEffectType;
        var dir = new Vector3(baseEntitys[0].transform.position.x - nowEntity.transform.position.x, 0, 0);
        foreach (var entity in baseEntitys)
        {
            if (type == SkillEffectType.Hit)
                EffectManager.Instance.CreateEffect(path, entity.transform.position); // 즉발 피격 이펙트 생성  
            else
                EffectManager.Instance.CreateEffect(path, nowEntity.transform.position, entity.transform.position, dir); // 투사체나 캐스팅 이펙트 생성  
        }
        baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerUp(); });
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.attackSfx, AudioInfo.Instance.attackSfxVolume);
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
            if (sprites[i] != null && layers[i] != 0)
            {
                layers[i] = sprites[i].sortingOrder;
                sprites[i].sortingOrder += 50;
            }
        }

        if (BattleManager.Instance.blackOutImage != null)
        {
            BattleManager.Instance.blackOutImage.SetActive(true);
        }
        else
        {
            BattleManager.Instance.InstantiateBlackOutImage();
            BattleManager.Instance.blackOutImage.SetActive(true);
        }
    }

    public override void LayerDown()
    {
        if (this == null || gameObject == null) return;
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        if (sprites != null)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                sprites[i].sortingOrder = layers[i];
            }

            if (BattleManager.Instance.blackOutImage != null)
            {
                BattleManager.Instance.blackOutImage.SetActive(false);
            }
            else
            {
                BattleManager.Instance.InstantiateBlackOutImage();
            }
        }
    }

    public override void ActionEvent()
    {
        action.Invoke();
    }

    public override void ActionEndEvent()
    {
        if (this == null || gameObject == null) return;
        base.ActionEndEvent();
        LayerDown();
        if (targetEntity != null)
        {
            targetEntity.characterAnimationController.LayerDown();
        }
        else
            baseEntitys.ForEach(baseEntity => { baseEntity.characterAnimationController.LayerDown(); });
        BattleManager.Instance.EndTurn(true);
    }
}

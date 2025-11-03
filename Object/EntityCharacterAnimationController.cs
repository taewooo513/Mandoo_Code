using System;
using System.Collections.Generic;
using UnityEngine;

public class EntityCharacterAnimationController : MonoBehaviour
{
    protected Animator animator;
    protected Action action;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void SetTrigger(string triggerName)
    {
        
    }

    public virtual void FlipX(bool isFlip)
    {
        
    }
    public virtual void Attack(Action action, BaseEntity baseEntity, Skill skill)
    {

    }
    public virtual void Attack(Action action, List<BaseEntity> baseEntitys, Skill skill)
    {

    }
    public virtual void Damaged()
    {

    }
    public virtual void Die()
    {

    }
    public virtual void DieEvent()
    {

    }
    public virtual void LayerUp()
    {
    }
    public virtual void LayerDown()
    {
    }
    public virtual void ActionEvent()
    {

    }
    public virtual void ActionEndEvent()
    {
    }
}

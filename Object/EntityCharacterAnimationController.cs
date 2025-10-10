using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EntityCharacterAnimationController : MonoBehaviour
{
    protected Animator animator;
    protected Action action;

    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public virtual void Attack(Action action, BaseEntity baseEntity)
    {

    }
    public virtual void Attack(Action action, List<BaseEntity> baseEntitys)
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

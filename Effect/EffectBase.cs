using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // public void Init()
    // {
    //     animator = GetComponent<Animator>();
    // }

    protected virtual void Update()
    {
        IsDestroy();
    }

    protected virtual void IsDestroy()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            Destroy(gameObject);
    }
}

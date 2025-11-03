using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastingEffect : EffectBase
{
    public GameObject hitEffect;
    private Vector3 _targetPosition;

    public void Init(Vector3 targetPosition)
    {
        _targetPosition = targetPosition;
    }
    protected override void Update()
    {
        base.Update();
        base.IsDestroy();
    }

    private void OnDestroy()
    {
        EffectManager.Instance.CreateEffect(hitEffect, _targetPosition);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEffect : EffectBase
{
    Vector3 dir;
    public float speed;
    public Vector3 targetPos;
    public GameObject hitEffect;
    protected override void Update()
    {
        Vector3 vec = transform.position;
        vec += dir;
        transform.position = vec;

        base.Update();
    }

    public void Setting(Vector3 targetPosition)
    {
        targetPos = targetPosition;
    }

    protected override void IsDestroy()
    {
        if (dir.x == -1)
        {
            if (targetPos.x <= transform.position.x)
            {
                //EffectManager.Instance.Add(hitEffect, targetPos);
                Destroy(this);
            }
        }
        else
        {
            if (targetPos.x >= transform.position.x)
            {
                //EffectManager.Instance.Add(hitEffect, targetPos);
                Destroy(this);
            }
        }
    }
}

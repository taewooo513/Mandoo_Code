using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEffect : EffectBase
{
    public Vector3 direction;
    public float speed;
    public Vector3 targetPosition;
    public GameObject hitEffect;
    
    protected override void Update()
    {
        Move();
        base.Update();
    }

    public void Init(Vector3 targetPosition, Vector3 direction)
    {
        speed = 10f;
        this.targetPosition = targetPosition;
        this.direction = direction;
    }

    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    protected override void IsDestroy()
    {
        if (direction.x == -1)
        {
            if (targetPosition.x >= transform.position.x)
            {
                Destroy(gameObject);
                //EffectManager.Instance.CreateEffect(hitEffect, transform.position);
            }
        }
        else
        {
            if (targetPosition.x <= transform.position.x)
            {
                Destroy(gameObject);
                //EffectManager.Instance.CreateEffect(hitEffect, transform.position);
            }
        }
    }
    
    private void OnDestroy()
    {
        EffectManager.Instance.CreateEffect(hitEffect, transform.position);
    }
}
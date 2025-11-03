using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    public void CreateEffect(string effectPath, Vector3 spawnPosition, Vector3 targetPosition, Vector3 direction) // 투사체 이펙트를 생성할 때 호출
    {
        var resource = Resources.Load<GameObject>(effectPath);
        if (resource == null) return;
        
        var effectPrefab = Instantiate(resource, spawnPosition, Quaternion.identity);
        
        var projectile = effectPrefab.GetComponent<MovingEffect>();
        if (projectile == null) return;
        projectile.Init(targetPosition, direction);
        
        var cast = effectPrefab.GetComponent<CastingEffect>();
        if (cast == null) return;
        cast.Init(targetPosition);
    }

    public void CreateEffect(string effectPath, Vector3 spawnPosition) // 피격 이펙트를 생성할 때 호출
    {
        var resource = Resources.Load<GameObject>(effectPath);
        if (resource == null) return;
        
        var effectPrefab = Instantiate(resource, spawnPosition, Quaternion.identity);
        var effect = effectPrefab.GetComponent<EffectBase>();
    }
    
    public void CreateEffect(GameObject effect, Vector3 spawnPosition) // 연계 이펙트를 생성할 때 호출
    {
        var effectPrefab = Instantiate(effect, spawnPosition, Quaternion.identity);
    }
}

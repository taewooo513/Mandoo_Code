using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillEffect
{
    private EffectData datas;
    protected EffectType effectType;
    public BuffType buffType;
    public DeBuffType debuffType;
    public string effectName;
    public string effectIconPath;
    protected float adRatio;
    protected int constantValue;
    protected int duration;
    public EffectType GetEffectType() { return effectType; }

    public void Init(EffectData datas)
    {
        this.datas = datas;
        effectType = datas.effectType;
        buffType = datas.buffType;
        debuffType = datas.debuffType;
        effectName = datas.effectName;
        effectIconPath = datas.effectIconPath;
        adRatio = datas.adRatio;
        constantValue = datas.constantValue;
        duration = datas.duration;
    }
    public virtual void ActiveEffect(BaseEntity actionEntity, BaseEntity targetEntity)
    {
    }
    public virtual void ActiveEffects(BaseEntity actionEntity, List<BaseEntity> targetEntitys)
    {
    }
}

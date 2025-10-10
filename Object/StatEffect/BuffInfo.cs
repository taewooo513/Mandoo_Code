using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffInfo //상태이상 효과 정보 리스트
{
    public BuffType buffType;
    public DeBuffType deBuffType;
    public int duration;
    public int constantValue;
    BaseEntity baseEntity;
    public float adRatio;
    public void Init(float adRatio, int duration, int constantValue, BaseEntity baseEntity, BuffType buffType, DeBuffType deBuffType) //스킬 사용할 때 호출
    {
        this.duration = duration;
        this.constantValue = constantValue;
        this.baseEntity = baseEntity;
        this.buffType = buffType;
        this.deBuffType = deBuffType;
        this.adRatio = adRatio;
    }
}
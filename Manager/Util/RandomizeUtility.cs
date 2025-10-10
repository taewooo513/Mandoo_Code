using System.Collections.Generic;
using UnityEngine;
using System;

public static class RandomizeUtility
{
    public static int TryGetRandomPlayerIndexByWeight(List<float> weights) // 가중치 리스트를 받아온 후, 가중치에 기반한 확률대로 playerIndex 뽑기
    {
        float total = 0f;
        int playerIndex = -1;

        foreach (var weight in weights)
            total += weight;

        float rand = UnityEngine.Random.value * total;

        int i = 0;
        foreach (var weight in weights)
        {
            rand -= weight; // 지정된 랜덤값에서 가중치를 반복해서 빼줌.

            if (rand <= 0f) // 음수가 될 경우
            {
                playerIndex = i; // playerIndex 뽑음. 몇 번째 가중치인지 대입(playerIndex). 확률이 높으면 -되는 숫자도 커지니, 뽑힐 확률이 높아짐.
                break;
            }
            i++;
        }
        return playerIndex;
    }

    public static Skill GetRandomSkillByWeight(List<Skill> skills) //스킬 가중치 뽑기
    {
        float total = 0f;
        var pickedSkill = new Skill();

        foreach (var skill in skills)
            total += skill.addedWeight;

        float rand = UnityEngine.Random.value * total;

        foreach (var skill in skills)
        {
            rand -= skill.addedWeight; // 지정된 랜덤값에서 가중치를 반복해서 빼줌.

            if (rand <= 0f)
            {
                pickedSkill = skill;
                break;
            }
        }
        return pickedSkill;
    }
}

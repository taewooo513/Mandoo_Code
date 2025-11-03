using System.Collections;
using System.Collections.Generic;
using DataTable;
using Unity.VisualScripting;
using UnityEngine;

public static class Spawn
{
    public static BaseEntity PlayableCharacterCreate(int id) //캐릭터 생성
    {
        GameObject playableCharacter = Object.Instantiate(Resources.Load<GameObject>(Constants.Player + "playableCharacter"));
        GameManager.Instance.AddPlayer(playableCharacter.GetComponent<BaseEntity>()); //게임매니저 리스트에 플레이어 추가
        playableCharacter.GetComponent<PlayableCharacter>().Init(id); //새로 만들어지는 프리팹에 플레이어 넣어줌
        return playableCharacter.GetComponent<PlayableCharacter>();
    }

    public static void PlayableCharacterSpawn() //캐릭터 스폰(위치 지정)
    {
        Vector3 pos = new Vector3(1, 0, 0);
        Vector3 add = new Vector3(2, 0, 0);
        
        for (int i = 0; i < GameManager.Instance.playableCharacter.Count; i++) //현재 데리고 있는 플레이어 리스트만큼 카운트
        {
            pos -= add;
            if(GameManager.Instance.playableCharacter[i] != null)
                GameManager.Instance.playableCharacter[i].transform.position = pos; //게임매니저에 있는 플레이어를 화면에 호출
        }
    }

    public static List<BaseEntity> EnemySpawn(List<int> id) //적 생성
    {
        var enemies = new List<BaseEntity>();
        Vector3 pos = new Vector3(-1, 0, 0);
        Vector3 add = new Vector3(2, 0, 0);

        for (int i = 0; i < id.Count; i++) //적 특정 위치에 생성
        {
            pos += add;
            GameObject enemy = Object.Instantiate(Resources.Load<GameObject>(Constants.Enemy + "Enemy"), pos, Quaternion.identity);
            GameManager.Instance.AddEnemy(enemy.GetComponent<BaseEntity>()); //게임매니저에서 적 생성
            enemy.GetComponent<Enemy>().Init(id[i]); //소환하려는 적을 새로 만들어지는 프리팹에 넣어줌
            enemies.Add(enemy.GetComponent<Enemy>());
        }

        return enemies;
    }

    public static GameObject TresureChestCreate()
    {
        GameObject tresureChest = Object.Instantiate(Resources.Load<GameObject>(Constants.TreasureChest + "TreasureChest"));
        return tresureChest;
    }

    public static GameObject PMCNPCCreate()
    {
        GameObject pmcNpc = Object.Instantiate(Resources.Load<GameObject>(Constants.PmcNPC));
        return pmcNpc;
    }

    public static GameObject ShopNPCCreate()
    {
        GameObject shopNpc = Object.Instantiate(Resources.Load<GameObject>(Constants.ShopNPC));
        return shopNpc;
    }

    public static void DestroyGameObject(GameObject gameObject)
    {
        Object.Destroy(gameObject);
    }
}

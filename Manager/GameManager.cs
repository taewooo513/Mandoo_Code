using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private List<BaseEntity> _playableCharacter;
    public List<BaseEntity> PlayableCharacter => _playableCharacter;

    private List<BaseEntity> _enemyCharacter;
    public List<BaseEntity> EnemyCharacter => _enemyCharacter;

    private void Awake()
    {
        _playableCharacter = new List<BaseEntity>();
        _enemyCharacter = new List<BaseEntity>();
    }

    public void AddPlayer(BaseEntity baseEntity)
    {
        _playableCharacter.Add(baseEntity);
    }

    public void DeletePlayableCharacter(BaseEntity baseEntity)
    {
        Destroy(baseEntity);
        _playableCharacter.Remove(baseEntity);
    }
    public void AddEnemy(BaseEntity baseEntity)
    {
        _enemyCharacter.Add(baseEntity);
    }
    public bool HasPlayerById(int id)
    {
        return _playableCharacter.Exists(pc => pc.id == id);//중복체크용
    }
    public void RemovePlayer(int id)
    {
        _playableCharacter.RemoveAll(pc => pc.id == id);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartBattle();
        }
    }

    public void StartGame()
    {
        _playableCharacter.Clear();
        _enemyCharacter.Clear();
        //TODO:인벤토리 비우는 것도 필요함.
        SceneManager.LoadScene("1.Scenes/MapTest");
    }

    public void StartBattle()
    {
        BattleManager.Instance.BattleStartTrigger(_playableCharacter, _enemyCharacter);
    }

    public void EndGame()
    {
    }

    public void PlayableCharacterPosition(List<BaseEntity> playerPositionList) //캐릭터 스폰(위치 지정)
    {
        _playableCharacter = playerPositionList;
    }
}
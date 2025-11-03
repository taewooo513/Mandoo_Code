using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using DataTable;

public class BattleManager : Singleton<BattleManager>
{
    public List<BaseEntity> _playableCharacters;
    public List<BaseEntity> PlayableCharacters => _playableCharacters;

    public List<BaseEntity> _enemyCharacters;
    public List<BaseEntity> EnemyCharacters => _enemyCharacters;

    public bool isUseItem = false;
    private BaseEntity _nowTurnEntity;
    public BaseEntity NowTurnEntity { get { return _nowTurnEntity; } }
    private BattleData _battleData;
    private List<InGameItem> _equipItem = new();
    public bool isEndTurn = false;
    private bool _isExtraTurn;
    private System.Random _random = new System.Random();
    private Queue<BaseEntity> _turnQueue;
    public GameObject blackOutImage;
    public bool isBattleStarted = false;
    public bool isCorridorBattle = false;
    public int HitEnemyCount = 0;
    public event Action OnTurnEnded; // 턴 종료 시 알림 이벤트
    protected override void Awake()
    {
        base.Awake();
        _playableCharacters = new List<BaseEntity>();
        _enemyCharacters = new List<BaseEntity>();
        _turnQueue = new Queue<BaseEntity>();
        InstantiateBlackOutImage();
    }

    public void InstantiateBlackOutImage()
    {
        blackOutImage = Instantiate(Resources.Load<GameObject>("UIPrefabs/BlackOutImage"));
        blackOutImage.SetActive(false);
    }

    private float GetAverageSpeed()
    {
        float sum = 0;
        foreach (var item in _playableCharacters)
        {
            sum += item.entityInfo.GetTotalBuffStat().speed;
        }

        foreach (var item in _enemyCharacters)
        {
            sum += item.entityInfo.GetTotalBuffStat().speed;
        }

        return sum / (_playableCharacters.Count + _enemyCharacters.Count);
    }

    public void OnceUseItem() // 1회 아이템 사용시 
    {
        if (isBattleStarted == true)
        {
            isUseItem = true;
        }
        else
        {
            isUseItem = false;
        }
    }

    private void HasExtraTurn()
    {
        if (_isExtraTurn)
        {
            _isExtraTurn = false;
            return;
        }
        float averageSpeed = GetAverageSpeed();
        float targetFloat = (_nowTurnEntity.entityInfo.GetTotalBuffStat().speed - averageSpeed) * 0.1f;
        _isExtraTurn = targetFloat >= UnityEngine.Random.value;
    }

    public void BattleStartTrigger(List<BaseEntity> playerList, List<BaseEntity> enemyList, BattleData battleData = null)
    {
        _playableCharacters.Clear();
        Debug.Log("BattleStartTrigger");
        HitEnemyCount = 0;
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.battleStartSfx, AudioInfo.Instance.battleStartSfxVolume);
        AchievementManager.Instance.SetParam("receivedDamage", 0);
        foreach (var item in playerList)
        {
            _playableCharacters.Add(item);
            item.BattleStarted();
        }

        foreach (var item in enemyList)
        {
            _enemyCharacters.Add(item);
            item.BattleStarted();
        }
        _battleData = battleData;
        Tutorials.ShowIfNeeded<BattleTutorial>();
        UIManager.Instance.OpenUI<InGameBattleStartUI>(); //전투 시작 UI 출력
        isBattleStarted = true;
        _turnQueue.Clear();
        //Turn(false);
    }

    public void StartTurn()
    {
        Turn(false);
    }

    public void GetLowHpSkillWeight(out float playerSkillWeight, out float enemySkillWeight) //스킬 가중치
    {
        playerSkillWeight = 0.0f;
        enemySkillWeight = 0.0f;
        foreach (var item in _playableCharacters)
        {
            if (item.entityInfo.LowHPStatPlayer()) //플레이어블 캐릭터 체력이 40% 일때
            {
                playerSkillWeight += 0.3f; //플레이어블 캐릭터의 스킬 가중치 추가(공격/스킬 우선 사용)
            }
        }

        foreach (var item in _enemyCharacters)
        {
            if (item.entityInfo.LowHPStatEnemy()) //enemy 체력이 10% 이하일 때
            {
                enemySkillWeight += 0.3f; //enemy 캐릭터의 스킬 가중치 추가(보호/힐 우선 사용)
            }
        }
    }
    private void Turn(bool isExtra) //한 턴
    {
        //GameManager.Instance.playableCharacter = _playableCharacters;
        if (!isBattleStarted || _playableCharacters.Count == 0 || _enemyCharacters.Count == 0)
        {
            return;
        }

        if (_turnQueue.Count == 0)
        {
            SetTurnQueue();
        }


        while (_turnQueue.Count > 0)
        {
            _nowTurnEntity = _turnQueue.Peek();
            if (_nowTurnEntity == null || _nowTurnEntity.gameObject == null)
            {
                _turnQueue.Dequeue();
                continue;
            }

            bool isAlive = false;
            if (_nowTurnEntity is PlayableCharacter)
            {
                isAlive = _playableCharacters.Contains(_nowTurnEntity);
            }
            else
            {
                isAlive = _enemyCharacters.Contains(_nowTurnEntity);
            }

            if (!isAlive)
            {
                _turnQueue.Dequeue();
                continue;
            }

            break;
        }

        if (_turnQueue.Count == 0)
        {
            SetTurnQueue();
            if (_turnQueue.Count == 0) return;
            _nowTurnEntity = _turnQueue.Peek();
        }

        if (isExtra)
        {
            _nowTurnEntity.StartExtraTurn();
        }
        else
        {
            _nowTurnEntity.StartTurn();
        }
    }

    public void EndTurn(bool canHaveExtraTurn = false)
    {
        //GameManager.Instance.playableCharacter = _playableCharacters;
        if (_playableCharacters.Count == 0)
        {
            Lose();
        }
        else if (_enemyCharacters.Count == 0)
        {
            Win();
        }
        else
        {
            HasExtraTurn();
            if (canHaveExtraTurn && _isExtraTurn)
            {
                _nowTurnEntity.EndTurn();
            }
            else
            {
                _nowTurnEntity.EndTurn();
                if (_turnQueue != null)
                    _turnQueue.Dequeue();
            }
            DieEntity();
            OnTurnEnded?.Invoke();
            if (_playableCharacters.Count == 0)
            {
                Lose();
            }
            else if (_enemyCharacters.Count == 0)
            {
                Win();
            }
            else
            {
                StartCoroutine(NextTurn(canHaveExtraTurn && _isExtraTurn));
            }
        }
    }

    IEnumerator NextTurn(bool extraTurn)
    {
        yield return new WaitForSeconds(1f);
        //GameManager.Instance.playableCharacter = _playableCharacters;
        Turn(extraTurn);
    }
    //private void BattleRun()
    //{
    //    Debug.Log("전투회피!");
    //    //전투회피
    //    UIManager.Instance.OpenUI<InGameBattleRunButton>();
    //    EndBattle();
    //}

    private void Win()
    {
        Debug.Log("승리! 버닝썬");
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.battleClearSfx, AudioInfo.Instance.battleClearSfxVolume);

        if (_playableCharacters.Count == 1)
        {
            AchievementManager.Instance.SetParam("soloVictory", 1);
        }

        DropItem();
        AddWeaponExpOnWin();
        AchievementManager.Instance.AddParam("winCount", 1);
        UIManager.Instance.CloseUI<InGameEnemyUI>();
        UIManager.Instance.OpenUI<InGameVictoryUI>(); //승리 UI 출력
                                                      //GameManager.Instance.playableCharacter = _playableCharacters;

        List<BaseEntity> playerList = new();
        foreach (var item in _playableCharacters)
        {
            playerList.Add(item);
        }
        GameManager.Instance.PlayableCharacterPosition(playerList); //현재 플레이어 위치 넘기기

        EndBattle();

        if (MapManager.Instance.CurrentLocation is BattleRoom battleRoom)
        {
            battleRoom.BattleEnd();
        }
        else if (MapManager.Instance.CurrentLocation is VillageRoom villageRoom)
        {
            villageRoom.BattleEnd();
        }
    }

    private void Lose()
    {
        Debug.Log("패배...");
        if (HitEnemyCount == 0)
            AchievementManager.Instance.AddParam("noHitDefeat", 1);
        EndBattle();
        GameManager.Instance.GameFinish(false);
    }

    private IEnumerator PlayCorridorBGMAfterDelay(float delay) //통로 전투가 일어났을 경우, 전투 끝나고 통로BGM으로 바꾸기
    {
        yield return new WaitForSeconds(delay);
        AudioManager.Instance.PlayBGM(AudioInfo.Instance.corridorBGM, AudioInfo.Instance.corridorBGMVolume);
    }

    public void EndBattle()
    {
        //TODO: 전투가 끝났을 때 공통적으로 해야하는 것...?
        isUseItem = false;
        if (isCorridorBattle)
        {
            StartCoroutine(PlayCorridorBGMAfterDelay(3f));
            isCorridorBattle = false;
        }

        foreach (var item in _playableCharacters)
        {
            item.Release();
        }

        foreach (var item in _enemyCharacters)
        {
            item.Release();
        }

        isBattleStarted = false;

        _playableCharacters.Clear();
        _enemyCharacters.Clear();
        GameManager.Instance.enemyCharacter.Clear();
        if (AnalyticsManager.Instance.Step == 25)
            AnalyticsManager.Instance.SendEventStep(26);
    }

    private void AddWeaponExpOnWin()
    {
        var gameDataSkillExp = GameManager.Instance.gameDatas.GetGameData(GameManager.Instance.CurrentMapIndex).battleWinExp;
        foreach (var item in _playableCharacters) //전투 승리 시 아군 전체에게 숙련도 지금
        {
            if (item.entityInfo.equips[0] != null)
                item.entityInfo.equips[0].AddWeaponExp(gameDataSkillExp);
        }
    }
    private void SetTurnQueue() //한번 섞은 후, 순서별 정렬, 플레이어 우선 정렬
    {
        int n = _playableCharacters.Count;
        int m = _enemyCharacters.Count;
        List<BaseEntity> tempPlayerList = new();
        List<BaseEntity> tempEnemyList = new();

        foreach (var item in _playableCharacters)
        {
            tempPlayerList.Add(item);
        }

        foreach (var item in _enemyCharacters)
        {
            tempEnemyList.Add(item);
        }
        while (n > 1)
        {
            n--;

            int k = _random.Next(n + 1);
            (tempPlayerList[k], tempPlayerList[n]) = (tempPlayerList[n], tempPlayerList[k]);
        }

        while (m > 1)
        {
            m--;

            int k = _random.Next(m + 1);
            (tempEnemyList[k], tempEnemyList[m]) = (tempEnemyList[m], tempEnemyList[k]);
        }

        tempPlayerList.Sort((a, b) => b.entityInfo.GetTotalBuffStat().speed.CompareTo(a.entityInfo.GetTotalBuffStat().speed));
        tempEnemyList.Sort((a, b) => b.entityInfo.GetTotalBuffStat().speed.CompareTo(a.entityInfo.GetTotalBuffStat().speed));
        while (tempPlayerList.Count != 0 || tempEnemyList.Count != 0)
        {
            if (tempPlayerList.Count == 0)
            {
                foreach (var item in tempEnemyList)
                {
                    _turnQueue.Enqueue(item);
                }

                tempEnemyList.Clear();
                break;
            }

            if (tempEnemyList.Count == 0)
            {
                foreach (var item in tempPlayerList)
                {
                    _turnQueue.Enqueue(item);
                }

                tempPlayerList.Clear();
                break;
            }

            if (tempPlayerList[0].entityInfo.GetTotalBuffStat().speed >= tempEnemyList[0].entityInfo.GetTotalBuffStat().speed)
            {
                _turnQueue.Enqueue(tempPlayerList[0]);
                tempPlayerList.RemoveAt(0);
            }
            else
            {
                _turnQueue.Enqueue(tempEnemyList[0]);
                tempEnemyList.RemoveAt(0);
            }
        }
    }

    //특정 Entity를 보내면 Position을 return합니다.
    public int? FindEntityPosition(BaseEntity baseEntity)
    {
        if (baseEntity == null) return null;
        if (baseEntity is PlayableCharacter)
        {
            if (isBattleStarted == true)
            {
                return _playableCharacters.IndexOf(baseEntity);
            }

            return GameManager.Instance.playableCharacter.IndexOf(baseEntity);
        }

        return _enemyCharacters.IndexOf(baseEntity);
    }

    public void AttackEntity(BaseEntity baseEntity)
    {
        baseEntity.Damaged(_nowTurnEntity.entityInfo.GetTotalBuffStat().attackDmg);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < _enemyCharacters.Count; i++)
        {
            _enemyCharacters[i].Release();
        }
        for (int i = 0; i < _playableCharacters.Count; i++)
        {
            _playableCharacters[i].Release();
        }
    }

    //public List<BaseEntity> SelectEntityRange(List<int> targetPos, BaseEntity tagetEntity, List<BaseEntity> tagetList)
    //{
    //    List<BaseEntity> result = new List<BaseEntity>();
    //    int index = Utillity.GetIndexInListToObject<BaseEntity>(tagetList, tagetEntity);
    //    if (tagetList.Count < index)
    //    {
    //        return result;
    //    }

    //    for (int i = 0; i < targetPos.Count; i++)
    //    {
    //        if (targetPos[i] < index)
    //        {
    //            continue;
    //        }
    //    }
    //}

    //index와 대미지를 넣으면 공격합니다.

    public void AttackEntity(int index, float attackDamage)
    {
        if (_nowTurnEntity is PlayableCharacter)
        {
            if (index < 0 || index >= _enemyCharacters.Count)
            {
                Debug.LogWarning($"[AttackEntity] 잘못된 enemy index: {index}, Count: {_enemyCharacters.Count}");
                return;
            }
            _enemyCharacters[index].Damaged(attackDamage);
        }
        else
        {
            if (index < 0 || index >= _playableCharacters.Count)
            {
                Debug.LogWarning($"[AttackEntity] 잘못된 enemy index: {index}, Count: {_enemyCharacters.Count}");
                return;
            }
            _playableCharacters[index].Damaged(attackDamage);
        }
    }

    public void DieEntity()
    {
        for (int i = 0; i < PlayableCharacters.Count;)
        {
            if (PlayableCharacters[i].entityInfo.isDie)
            {
                PlayableCharacters[i].OnDied?.Invoke(PlayableCharacters[i]);
            }
            else
            {
                i++;
            }
        }
        for (int i = 0; i < EnemyCharacters.Count;)
        {
            if (EnemyCharacters[i].entityInfo.isDie)
            {
                EnemyCharacters[i].OnDied?.Invoke(EnemyCharacters[i]);
            }
            else
            {
                i++;
            }
        }
    }

    //범위 공격에 적합한 타입입니다.
    public void AttackEntity(List<int> indexList, float attackDamage)
    {
        if (_nowTurnEntity is PlayableCharacter)
        {
            foreach (var index in indexList)
            {
                _enemyCharacters[index].Damaged(attackDamage);
            }
        }
        else
        {
            foreach (var index in indexList)
            {
                _enemyCharacters[index].Damaged(attackDamage);
            }
        }
    }

    //플레이어블 캐릭터 리스트에 캐릭터를 추가합니다.
    public void AddPlayableCharacter(PlayableCharacter playableCharacter)
    {
        _playableCharacters.Add(playableCharacter);
    }

    //적 캐릭터 리스트에 캐릭터를 추가합니다.
    public void AddEnemyCharacter(Enemy enemy)
    {
        _enemyCharacters.Add(enemy);
    }

    //안쓰지 않을까요?
    public void AttackEnemy(int damageValue, int index)
    {
        _enemyCharacters[index].Damaged(damageValue);
    }

    //안쓰지 않을까요?
    public void AttackPlayer(int damageValue, int index)
    {
        _playableCharacters[index].Damaged(damageValue);
    }

    public int GetTotalNumOfPlayerCharacters() // 적과 조우한 플레이어 캐릭터 수 반환
    {
        return _playableCharacters.Count;
    }

    //자기 자신->this, enablePos List 보내주세요 -> 쓸 수 있는지 안되는지 알려줍니다.
    public bool IsEnablePos(BaseEntity entity, List<int> posList)
    {
        if (_nowTurnEntity is PlayableCharacter)
        {
            foreach (var item in posList)
            {
                if (_playableCharacters.Count > item && _playableCharacters[item] == entity)
                {
                    return true;
                }
            }
        }
        else
        {
            foreach (var item in posList)
            {
                if (_enemyCharacters.Count > item && _enemyCharacters[item] == entity)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsTargetInList(List<int> targetPos)
    {
        if (_nowTurnEntity is PlayableCharacter)
        {
            foreach (var item in targetPos)
            {
                if (_enemyCharacters.Count > item)
                {
                    return true;
                }
            }

            return false;
        }
        foreach (var item in targetPos)
        {
            if (_playableCharacters.Count > item)
            {
                return true;
            }
        }
        return false;
    }

    //플레이어 위치 받아오는 함수
    public List<(int, int)> GetPlayerPosition()
    {
        return new List<(int, int)>(); //임시: Item1 = 위치값; Item2 = id 값
    }

    //적 위치 받아오는 함수
    public List<(int, int)> GetEnemyPosition()
    {
        return new List<(int, int)>(); //임시: Item1 = 위치값; Item2 = id 값
    }


    //날 것의 스킬 범위를 던져주면 "때릴 수 있는" 적의 위치 리스트를 반환합니다. 범위 공격에 적합합니다.
    public List<int> GetPossibleSkillRange(List<int> skillRange, BaseEntity targetType)
    {
        List<int> possibleSkillRange = new List<int>();
        if (targetType is PlayableCharacter)
        {
            foreach (var pos in skillRange)
            {
                if (PlayableCharacters.Count > pos && PlayableCharacters[pos] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }
        else
        {
            foreach (var pos in skillRange)
            {
                if (EnemyCharacters.Count > pos && EnemyCharacters[pos] != null)
                {
                    possibleSkillRange.Add(pos);
                }
            }
        }

        return possibleSkillRange;
    }

    public void SwitchPlayerPosition(PlayableCharacter playableCharacterA, PlayableCharacter playableCharacterB)
    {
        int indexA = -1;
        int indexB = -1;

        for (int i = 0; i < _playableCharacters.Count; i++)
        {
            if (_playableCharacters[i] == playableCharacterA)
            {
                indexA = i;
            }
            else if (_playableCharacters[i] == playableCharacterB)
            {
                indexB = i;
            }
        }

        if (indexA == -1 || indexB == -1)
        {
            Debug.LogWarning("SwitchPlayerPosition: indexA or indexB is -1");
            return;
        }

        _playableCharacters[indexA] = playableCharacterB;
        _playableCharacters[indexB] = playableCharacterA;
        SwapEntityTransform(playableCharacterA, playableCharacterB);
        foreach (var item in _playableCharacters)
        {
            Debug.Log(item.entityInfo.name);
        }
    }

    private void DropItem()
    {
        if (_battleData == null) return;
        var rewardGroupId = _battleData.rewardId;

        var dropGoldCount = _battleData.dropGold;
        var dropItem = _battleData.dropId; //드랍하는 아이템id (골드x)
        var battleDropProb = _battleData.dropProb; //아이템 드랍 확률 (ex : 0.25)
        var goldRandomRatio = UnityEngine.Random.Range(0.9f, 1.1f); //0.9~1.1 사이 랜덤 난수 반환, 골드 떨어지는 랜덤 개수
        var randomGoldDropCount = (int)(dropGoldCount * goldRandomRatio); //실제로 떨어지는 금화 개수
        var randomPercentage = UnityEngine.Random.Range(0f, 100f);

        InGameItem[] items = new InGameItem[10];
        items[0] = InGameItemManager.Instance.AddItem(1001, randomGoldDropCount); //랜덤 개수대로 보상 ui에 골드 아이템 추가
        if (randomPercentage < battleDropProb) //드랍 확률대로 아이템 떨어짐 (ex : 0.25% 확률로 아이템 떨어짐)
        {
            items[1] = InGameItemManager.Instance.AddItem(dropItem, 1); //보상 ui에 아이템 추가
        }

        _equipItem = GameManager.Instance.deadEquipWeapons;
        if (_equipItem.Count != 0)
        {
            int idx = 0;
            if (items[1] != null)
                idx = 2;
            else //살짝 위험한 코드
                idx = 1;

            for (int i = 0; i < _equipItem.Count; i++) //'죽은 플레이어가 죽기 전 가지고있던 장비' 리스트 순회하면서
            {
                if (idx >= items.Length) break; //최대 개수 넘어가면 break

                if (_equipItem[i] != null)
                {
                    items[idx] = _equipItem[i]; //보상 ui에 장비 추가하기
                    idx++;
                }
            }
        }
        UIManager.Instance.UIGet<InGameVictoryUI>().Setting(items); //ui에 아이템 넣어주기

        _equipItem.Clear(); //장비 리스트 초기화
    }

    //게임 오브젝트의 위치를 바꿔줍니다.
    private void SwapEntityTransform(BaseEntity entityA, BaseEntity entityB)
    {
        (entityB.transform.position, entityA.transform.position) =
            (entityA.transform.position, entityB.transform.position);
    }

    //entity의 위치를 desiredPosition index와 변경합니다.
    public void SwitchPosition(BaseEntity entity, int desiredPosition)
    {
        if (entity is PlayableCharacter)
        {
            var index = -1;
            foreach (var character in _playableCharacters)
            {
                if (character == entity)
                {
                    index = _playableCharacters.IndexOf(character);
                    break;
                }
            }

            if (index == -1)
            {
                return;
            }

            // 구조 분해를 이용한 스왑 예시: 고맙다! 라이더야!
            // (a, b) = (b, a) 형태로 한 줄로 스왑 가능
            (_playableCharacters[index], _playableCharacters[desiredPosition]) =
                (_playableCharacters[desiredPosition], _playableCharacters[index]);
            SwapEntityTransform(_playableCharacters[index], _playableCharacters[desiredPosition]);
        }
        else
        {
            var index = -1;
            foreach (var character in _enemyCharacters)
            {
                if (character == entity)
                {
                    index = _enemyCharacters.IndexOf(entity);
                    break;
                }
            }

            if (index == -1 || index == desiredPosition || desiredPosition >= _enemyCharacters.Count)
            {
                return;
            }

            if (index + 1 == desiredPosition || index - 1 == desiredPosition)
            {
                (_enemyCharacters[index], _enemyCharacters[desiredPosition]) =
                    (_enemyCharacters[desiredPosition], _enemyCharacters[index]);
                SwapEntityTransform(_enemyCharacters[index], _enemyCharacters[desiredPosition]);
            }
            else
            {
                //index가 현재 바꾸고 싶어하는 entity의 위치
                if (index > desiredPosition)
                {
                    (_enemyCharacters[index - 1], _enemyCharacters[index]) = (
                        _enemyCharacters[index], _enemyCharacters[index - 1]);
                    SwapEntityTransform(_enemyCharacters[index - 1], _enemyCharacters[index]);
                }
                else
                {
                    (_enemyCharacters[index + 1], _enemyCharacters[index]) = (
                        _enemyCharacters[index], _enemyCharacters[index + 1]);
                    SwapEntityTransform(_enemyCharacters[index + 1], _enemyCharacters[index]);
                }
            }
        }
    }

    public List<float> GetWeightList(bool isPlayer, List<int> targetRange) //타겟 가중치 리스트
    {
        // 공격류 스킬
        if (isPlayer)
        {
            foreach (var target in targetRange)
            {
                _playableCharacters[target].entityInfo.GetPlayableTargetWeight(); //playable 가중치 가져오기
            }

            return GenerateWeightListUtility.GetWeights();
        }
        //지원류 스킬
        foreach (var item in _enemyCharacters)
        {
            item.entityInfo.GetEnemyTargetWeight(); //enemy 가중치 가져오기
        }
        return GenerateWeightListUtility.GetWeights();
    }


    public void EntityDead(BaseEntity entity)
    {
        if (entity == null) return;

        var index = FindEntityPosition(entity);
        if (index == null) return;

        if (entity is PlayableCharacter)
        {
            if (index >= _playableCharacters.Count) return;

            for (int i = (int)index; i < _playableCharacters.Count - 1; i++)
            {
                SwitchPosition(entity, i + 1);
            }

            RemoveDeadEntityFromTurnQueue(entity);
            _playableCharacters.RemoveAt(_playableCharacters.Count - 1);
            entity.Release();

            if (entity.gameObject != null)
            {
                Destroy(entity.gameObject, 1.0f);
            }
            return;
        }

        if (index >= _enemyCharacters.Count) return;

        for (int i = (int)index; i < _enemyCharacters.Count - 1; i++)
        {
            SwitchPosition(entity, i + 1);
        }

        RemoveDeadEntityFromTurnQueue(entity);
        //TODO: 이후 적 사망시 보상 연결은 여기서? 아니면 Enemy에서?
        entity.Release();
        _enemyCharacters.RemoveAt(_enemyCharacters.Count - 1);

        if (entity.gameObject != null)
        {
            Destroy(entity.gameObject, 1.0f);
        }
    }

    private void RemoveDeadEntityFromTurnQueue(BaseEntity entity)
    {
        int loopTime = _turnQueue.Count;
        for (int i = 0; i < loopTime; i++)
        {
            var item = _turnQueue.Dequeue();
            if (item == entity)
            {
                continue;
            }
            _turnQueue.Enqueue(item);
        }
    }

}
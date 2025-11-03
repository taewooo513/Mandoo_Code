using Cysharp.Threading.Tasks;
using DataTable;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using UGS;
using UnityEngine;
using UnityEngine.Networking;

public class UserData
{
    public readonly string firebaseToken;
    public UserData(string token)
    {
        firebaseToken = token;
    }
}

public class DataManager : Singleton<DataManager>
{
    public SkillDatas Skill;
    public MercenaryDatas Mercenary; //Player로 변경해도 됨.
    public EnemyDatas Enemy;
    public EffectDatas Effect;
    public WeaponDatas Weapon;
    public BattleDatas Battle;
    public ConsumableDatas Consumable;
    public ItemDatas Item;
    public GameDatas Game;
    public StoreDatas Store;
    public MapDatas Map;
    public RewardDatas Reward;
    public AchievementDatas _achievement;
    public LoadoutDatas LoadOut;
    public SaveData SaveData;
    private UserData userData;

    private bool _isInitialized = false;

    private string _achievementPath = Path.Combine(Application.streamingAssetsPath, "SaveDataJson.json");
    private string _achievementPersistentPath;

    protected override void Awake()
    {
        base.Awake();
        _achievementPersistentPath = Path.Combine(Application.persistentDataPath, "SaveDataJson.json");
    }

    public async UniTask Initialize()
    {
        if (_isInitialized) return;
        UnityGoogleSheet.LoadAllData();
        Skill = new SkillDatas();
        Mercenary = new MercenaryDatas();
        Enemy = new EnemyDatas();
        Effect = new EffectDatas();
        Weapon = new WeaponDatas();
        Battle = new BattleDatas();
        Consumable = new ConsumableDatas();
        Map = new MapDatas();
        Reward = new RewardDatas();
        Item = new ItemDatas();
        Game = new GameDatas();
        Store = new StoreDatas();
        _achievement = new AchievementDatas();
        LoadOut = new LoadoutDatas();
        SaveData = new SaveData();
        _isInitialized = true;

        await LoadSaveData();
    }

    private async UniTask LoadSaveData()
    {
        //if (!File.Exists(_achievementPersistentPath))
        //{
        //    if (!File.Exists(_achievementPath))
        //    {
        //        MakeNewSaveFile();
        //    }
        //    File.Copy(_achievementPath, _achievementPersistentPath, true);
        //}
        Debug.Log("Lodding");
        await GetJson();
    }

    public T LoadJsonData<T>(string path) where T : class
    {
        if (!File.Exists(path))
        {
            Debug.LogError($"JSON 파일이 존재하지 않습니다: {path}");
            return null;
        }

        string json = File.ReadAllText(path);

        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError($"JSON 파일이 비어있습니다: {path}");
            return null;
        }

        Debug.Log($"JSON 내용: {json}");

        try
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch (JsonException ex)
        {
            Debug.LogError($"JSON 역직렬화 실패: {ex.Message}");
            return null;
        }

        // string json = File.ReadAllText(path);
        // return JsonConvert.DeserializeObject(json) as T;
    }

    public void Save(bool isPersistent = true)
    {
        string json = JsonConvert.SerializeObject(SaveData, Formatting.Indented);
        if (!isPersistent)
        {
            File.WriteAllText(_achievementPath, json);
        }
        File.WriteAllText(_achievementPersistentPath, json);
        StartCoroutine(PostJson(Constants.AwsPath + "setData", json));
#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL 브라우저에선 세이브 flush를 보장하도록
        PlayerPrefs.Save(); // (파일도 함께 동기화되도록 트리거하는 관용적 호출)
#endif
        //1. 스테이지 데이터를 받아오기,
        //2. 세팅 데이터를 받아오기,
        //3. 로드아웃 데이터를 받아오기,
        //4. DataManager에 있는 SaveData의 값을 변경한 후, Save함.
    }

    private void MakeNewSaveFile()
    {
        Debug.Log("Make New Save File");
        ClearJson();
        MakeStageSave();
        MakeAchievementSave();
        Save(false);
    }

    private void MakeStageSave()
    {
        int stageCount = MapData.MapDataList.Count;
        for (int i = 0; i <= stageCount; i++)
        {
            StageSaveData stageSave;
            if (i == 0)
            {
                stageSave = new StageSaveData(0);
            }
            else
            {
                stageSave = new StageSaveData(MapData.MapDataList[i - 1].id);
            }
            SaveData.StageSaveList.Add(stageSave);
        }
    }

    private void MakeAchievementSave()
    {
        int achievementCount = Achievements.AchievementsList.Count;

        for (int i = 0; i < achievementCount; i++)
        {
            var temp = Achievements.AchievementsList[i];
            var achievement = new AchievementSaveData(temp.Id, temp.param, 0, false);
            SaveData.AchievementSaveList.Add(achievement);
        }
    }
    private void ClearJson()
    {
        File.WriteAllText(_achievementPath, string.Empty);
    }

    private IEnumerator Load()
    {
        Debug.Log("Load Data");
        if (File.Exists(_achievementPersistentPath))
        {
            SaveData = LoadJsonData<SaveData>(_achievementPersistentPath);
            yield break;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL에선 URL로 접근해야 함

        // 
        using (var req = UnityWebRequest.Get(_achievementPath))
        {
            yield return req.SendWebRequest();
            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Failed to load default save from StreamingAssets: {req.error}\nURL: {_achievementPath}");
                MakeNewSaveFile();
                yield break;
            }

            string json = req.downloadHandler.text;
            SafeWritePersistent(json);
            try
            {
                SaveData = JsonConvert.DeserializeObject<SaveData>(json);
            }
            catch (JsonException ex)
            {
                Debug.LogError($"JSON 역직렬화 실패: {ex.Message}");
                yield break;
            }
        }
#else
        // Editor/Standalone에서는 파일 IO 가능
        string json = File.ReadAllText(_achievementPath);
        SafeWritePersistent(json);
        try
        {
            SaveData = JsonConvert.DeserializeObject<SaveData>(json);
        }
        catch (JsonException ex)
        {
            Debug.LogError($"JSON 역직렬화 실패: {ex.Message}");
            yield break;
        }
#endif
        Debug.Log("Load Completed");
        yield break;
    }

    private void SafeWritePersistent(string json)
    {
        var dir = Path.GetDirectoryName(_achievementPersistentPath);
        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
        File.WriteAllText(_achievementPersistentPath, json);
    }

    //스테이지 클리어 시 이 곳에 정보를 보내주세요. 단, 튜토리얼 스테이지는 0번입니다.
    public void OnStageClear(int stageNumber)
    {
        StageSaveData stageSave = SaveData.StageSaveList.Find(x => x.StageID == stageNumber);
        stageSave.IsCleared = true;
        stageSave.ClearCount++;
        Save();
    }

    //세팅 값이 변경되면 이 곳에 정보를 보내주세요. 세 개의 정보를 항상 다 보내야합니다.
    public void OnSettingsChanged(float masterVolume, float bgmVolume, float soundEffectVolume, bool isMuted)
    {
        SaveData.SettingSave.MasterVolume = masterVolume;
        SaveData.SettingSave.BgmVolume = bgmVolume;
        SaveData.SettingSave.SoundEffectVolume = soundEffectVolume;
        SaveData.SettingSave.IsMuted = isMuted;
        Save();
    }

    //로드 아웃을 구매했다면 이 곳에 정보를 보내주세요.
    public void OnLoadOutBought(int unlockPoint, int loadOutID)
    {
        SaveData.LoadOutSave.LoadOutUnlockPoint = unlockPoint;
        if (!SaveData.LoadOutSave.LoadOutID.Contains(loadOutID))
        {
            SaveData.LoadOutSave.LoadOutID.Add(loadOutID);
        }
        Save();
    }

    //로드 아웃을 바꾼 뒤 게임을 시작했다면 이 곳에 정보를 보내주세요.
    public void OnLoadOutChanged(List<int> currentLoadOutID)
    {
        SaveData.LoadOutSave.CurrentLoadOutID = currentLoadOutID;
        Save();
    }

    //로드아웃 포인트의 값이 변경되었다면 이 곳에 정보를 보내주세요.
    public void OnLoadOutPointChanged(int unlockPoint)
    {
        SaveData.LoadOutSave.LoadOutUnlockPoint = unlockPoint;
        Save();
    }

    public void OnAchievementCleared(int id)
    {
        SaveData.AchievementSaveList.Find(x => x.ID == id).IsCleared = true;
        Save();
    }

    public void OnAchievementCountChanged(int id, int count)
    {
        SaveData.AchievementSaveList.Find(x => x.ID == id).Count = count;
        Save();
    }

    IEnumerator PostJson(string url, string json)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
        using (UnityWebRequest request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
            }
        }
    }
    async UniTask GetJson()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(Constants.AwsPath + "getData"))
        {
            await request.SendWebRequest().ToUniTask();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;
                try
                {
                    SaveData = JsonConvert.DeserializeObject<SaveData>(json);
                }
                catch (Exception ex)
                {
                    MakeNewSaveFile();
                    Debug.Log(ex.Message);
                }

                Debug.Log("Succes");
            }
        }
    }
}
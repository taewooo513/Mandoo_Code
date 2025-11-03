using DataTable;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PMCHire : MonoBehaviour
{
    public static PMCHire Instance { get; private set; }

    public GameObject playerPrefab;
    public Vector3[] spawnPoints = new Vector3[4];

    private List<GameObject> spawnedPMCs = new List<GameObject>();

    [SerializeField] private PMCCardManager cardManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // 고용(소환)
    public bool SpawnPMC(int initID, int contractGold)
    {
        Debug.Log(initID + "번 PMC 고용 시도");
        // 1. GameManager 플레이어 리스트에서 중복 체크 (id 기준)
        if (GameManager.Instance.HasPlayerById(initID))
        {
            AudioManager.Instance.PlaySfx(AudioInfo.Instance.shopDenySfx, AudioInfo.Instance.shopDenySfxVolume);
            return false;
        }
        if (GameManager.Instance.playableCharacter.Count >= spawnPoints.Length)
        {
            AudioManager.Instance.PlaySfx(AudioInfo.Instance.shopDenySfx, AudioInfo.Instance.shopDenySfxVolume);
            Debug.Log("PMC 자리가 꽉찼습니다!");
            return false;
        }
        if (!InGameItemManager.Instance.IsUseGold(contractGold))
        {
            AudioManager.Instance.PlaySfx(AudioInfo.Instance.shopDenySfx, AudioInfo.Instance.shopDenySfxVolume);
            Debug.LogWarning("골드 부족! 고용실패!");
            return false;
        }
        AudioManager.Instance.PlaySfx(AudioInfo.Instance.shopBuySfx, AudioInfo.Instance.shopBuySfxVolume);
        InGameItemManager.Instance.UseGold(contractGold);
        GameObject pmc = Instantiate(playerPrefab, spawnPoints[GameManager.Instance.playableCharacter.Count], Quaternion.identity);
        UIManager.Instance.CloseUI<MapUI>();
        UIManager.Instance.OpenUI<InGameInventoryUI>();
        if (AnalyticsManager.Instance.Step == 15)
            AnalyticsManager.Instance.SendEventStep(16);

        UIManager.Instance.OpenUI<InGameUIManager>().RemoveSkillButton();

        var playable = pmc.GetComponent<PlayableCharacter>();
        if (playable != null)
        {
            playable.Init(initID);
            GameManager.Instance.AddPlayer(playable);
            return true;
        }

        Debug.LogError("PlayableCharacter 컴포넌트가 프리팹에 없습니다!");
        return false;

    }

    public void RemovePlayerAt(int index)
    {
        var list = GameManager.Instance.playableCharacter;


        if (index < 0 || index >= list.Count)
        {
            Debug.LogWarning($"잘못된 자리입니다!");
            return;
        }

        var playable = list[index];
        if (playable == null)
        {
            Debug.LogWarning($"이미 빈 자리이거나 입니다! 인덱스: {index}");
            return;
        }

        if (list.Count <= 1)
        {
            Debug.LogWarning("1명 이하는 해고할 수 없습니다!");
            return;
        }

        GameManager.Instance.RemovePlayer(playable.id);
        Destroy(playable.gameObject);

        UpdatePlayerPositions();
    }

    private void UpdatePlayerPositions()
    {
        for (int i = 0; i < GameManager.Instance.playableCharacter.Count; i++)
        {
            GameManager.Instance.playableCharacter[i].transform.position = spawnPoints[i]; // 위치 재조정
        }
    }
}


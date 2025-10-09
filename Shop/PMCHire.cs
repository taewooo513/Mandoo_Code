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
    public void SpawnPMC(int initID, int contractGold)
    {
        Debug.Log(initID + "번 PMC 고용 시도");
        // 1. GameManager 플레이어 리스트에서 중복 체크 (id 기준)
        if (GameManager.Instance.HasPlayerById(initID))
        {
            return;
        }
        if (GameManager.Instance.PlayableCharacter.Count >= spawnPoints.Length)
        {
            Debug.Log("PMC 자리가 꽉찼습니다!");
            return;
        }
        //bool goldUsed = InventoryManager.Instance.UseGole(contractGold);
        //if (!goldUsed)
        //{
        //    Debug.LogWarning("골드 부족! 고용실패!");
        //    return;
        //}

        // 3. 생성 및 등록
        GameObject pmc = Instantiate(playerPrefab, spawnPoints[GameManager.Instance.PlayableCharacter.Count], Quaternion.identity);


        var playable = pmc.GetComponent<PlayableCharacter>();
        if (playable != null)
        {
            playable.SetData(initID);
            GameManager.Instance.AddPlayer(playable);
            
        }
        else
        {
            Debug.LogError("PlayableCharacter 컴포넌트가 프리팹에 없습니다!");
        }
        PMCCardManager.Instance.RefreshCardsOnPanel();
    }

    public void RemovePlayerAt(int index)
    {
        var list = GameManager.Instance.PlayableCharacter;
        

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
        PMCCardManager.Instance.RefreshCardsOnPanel();
    }

    private void UpdatePlayerPositions()
    {
        for (int i = 0; i < GameManager.Instance.PlayableCharacter.Count; i++)
        {
            GameManager.Instance.PlayableCharacter[i].transform.position = spawnPoints[i]; // 위치 재조정
        }
    }  
}


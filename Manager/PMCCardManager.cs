using UnityEngine;

public class PMCCardManager : MonoBehaviour
{
    public static PMCCardManager Instance { get; private set; }

    public PMCInfo[] pmcCards;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RefreshCardsOnPanel()
    {
        pmcCards = GetComponentsInChildren<PMCInfo>(true);
        foreach (var card in pmcCards)
        {
            bool hasPlayer = GameManager.Instance.HasPlayerById(card.InitID);
            card.gameObject.SetActive(!hasPlayer);
        }
    }
}
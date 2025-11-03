using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PMCCardManager : MonoBehaviour
{
    [SerializeField] private Transform content;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private GameObject cardPrefab;
    private InGamePMCUI _inGamePMCUI;

    private List<GameObject> _cards = new();
    private PMCInfo[] pmcCards;
    private List<int> _ids = new();

    public void GetData(List<int> ids, InGamePMCUI inGamePmcui)
    {
        _ids = ids;
        _inGamePMCUI = inGamePmcui;
    }
    public void RefreshCardsOnPanel()
    {
        ClearCards();
        var rt = content.GetComponent<RectTransform>();
        rt.anchoredPosition = new Vector2(rt.sizeDelta.x, 0);
        var count = 0;
        foreach (var item in _ids)
        {
            var go = Instantiate(cardPrefab, content);
            if (go == null) continue;
            _cards.Add(go);
            var data = DataManager.Instance.Mercenary.GetMercenaryData(item);
            go.GetComponent<PMCInfo>().Init(data, _inGamePMCUI);
            count++;
        }
        // pmcCards = GetComponentsInChildren<PMCInfo>(true);
        // int count = 0;
        // foreach (var card in pmcCards)
        // {
        //     bool hasPlayer = GameManager.Instance.HasPlayerById(card.InitID);
        //     card.gameObject.SetActive(!hasPlayer);
        //     if (!hasPlayer) count++;
        // }

        content.GetComponent<RectTransform>().sizeDelta = new Vector2(content.GetComponent<RectTransform>().sizeDelta.x, count * 110);

        scrollRect.vertical = count > 4;
    }

    private void ClearCards()
    {
        if (_cards.Count == 0) return;
        foreach (var item in _cards)
        {
            Destroy(item);
        }
        _cards.Clear();
    }
}
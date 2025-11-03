using DataTable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// 상점에서 개별 아이템을 담당하는 UI 컴포넌트
public class ShopItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button buyButton;
    [SerializeField] private GameObject itemTooltip;
    
    private StoreData storeData;
    private int purchaseCount = 0; // 이미 구매한 개수
    // storeData.itemCount를 최대 구매 개수로 사용
    private int MaxBuyCount => storeData.itemCount;
    private InGameItem hoverItem;

    private void Awake()
    {
        itemTooltip.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        string itemName = hoverItem.GetItemInfo.name;
        string itemDesc = hoverItem.GetItemInfo.desc;
        itemTooltip.GetComponent<Tooltip>().SetTooltipText(itemName, itemDesc);
        itemTooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        itemTooltip.SetActive(false);
    }
    
    public void SetStoreData(StoreData data)
    {
        storeData = data;
        // 구매 개수는 상점(방)마다 0으로 초기화!
        purchaseCount = 0;
        // 아이템 정보 가져오고 가격 표시
        InGameItem inGameItem = InGameItemManager.Instance.AddItem(storeData.itemId, 1);
        itemNameText.text = inGameItem.GetItemInfo.price.ToString() + "G";
        hoverItem = inGameItem;
        // 아이콘 표시 (아이템 정보에 아이콘 경로가 있다면)
        iconImage.sprite = Resources.Load<Sprite>(inGameItem.GetItemInfo.iconPath);

        // 구매 버튼 리스너 등록 (중복 방지 위해 RemoveAllListeners)
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyItem);

        UpdateBuyButtonState();
    }

    private void UpdateBuyButtonState()
    {
        // 최대 구매 개수에 도달하면 비활성화
        if (purchaseCount >= MaxBuyCount)
        {
            buyButton.interactable = false;
        }
        else
        {
            buyButton.interactable = true;
        }
    }
    public void BuyItem()
    {
        // 최대 구매 도달 시 구매 불가
        if (purchaseCount >= MaxBuyCount)
        {
            AudioManager.Instance.PlaySfx(AudioInfo.Instance.shopDenySfx, AudioInfo.Instance.shopDenySfxVolume);
            Debug.Log("최대 구매 수량에 도달했습니다!");
            buyButton.interactable = false;
            return;
        }

        // 아이템 가격정보 (InGameItem 내부에서 가져옴)
        InGameItem inGameItem = InGameItemManager.Instance.AddItem(storeData.itemId, 1);
        int price = inGameItem.GetItemInfo.price;

        // 골드가 충분한지 확인
        if (InGameItemManager.Instance.IsUseGold(price))
        {
            AudioManager.Instance.PlaySfx(AudioInfo.Instance.shopBuySfx, AudioInfo.Instance.shopBuySfxVolume);
            InGameItemManager.Instance.UseGold(price);
            InGameItemManager.Instance.InsertItem(inGameItem);
            purchaseCount++;
            if (GameManager.Instance.CurrentMapIndex == 0)
                AnalyticsManager.Instance.SendEventStep(22);
            Debug.Log($"{inGameItem.GetItemInfo.name} 구매 성공! ({purchaseCount}/{MaxBuyCount})");
            UpdateBuyButtonState();
        }
        else
        {
            Debug.Log("골드가 부족합니다!");
        }
    }
}
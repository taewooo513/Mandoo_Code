using DataTable;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour
{
    [Header("common Data")]
    public int id;
    //public string iconPathString; 

    [Header("consumable Data")]

    public ConsumableType itemType;
    public int consumableSkillId;
    public string itemName;
    public string itemDescription;
    public int maxCount;
    public int price;

    [Header("weapon Data")]
    public int proficiencyLevel;
    public WeaponType weaponType;
    public int attack;
    public int defense;
    public int speed;
    public float evasion;
    public float critical;
    public int skillId;
    //public string gameObjectString; 

    private ConsumableData consumableData;
    private WeaponData weaponData;


    // 예시: UI 오브젝트들
    public TextMeshProUGUI itemNameText;
    public Image iconImage;
    public Button buyButton;

    public void SetConsumableData(ConsumableData data)
    {
        consumableData = data;
        id = data.id;
        consumableSkillId = data.consumableSkillId;
        //itemType = data.itemType;
        //itemName = data.itemName;
        //itemDescription = data.itemDescription;
        //maxCount = data.maxCount;
        //price = data.price;
        //iconPathString = data.iconPathString;

        // UI 반영
        //itemNameText.text = data.itemName;
        //iconImage.sprite = Resources.Load<Sprite>(data.iconPathString);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyConsumable);
    }

    public void SetWeaponData(WeaponData data)
    {
        weaponData = data;
        id = data.id;
        proficiencyLevel = data.proficiencyLevel;
        weaponType = data.weaponType;
        attack = data.attack;
        defense = data.defense;
        speed = data.speed;
        evasion = data.evasion;
        critical = data.critical;
        skillId = data.skillId;
        //gameObjectString = data.gameObjectString;
        //iconPathString = data.iconPathString;


        // UI 반영
        itemNameText.text = data.weaponType.ToString();
        //iconImage.sprite = Resources.Load<Sprite>(data.iconPathString);
        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(BuyWeapon);
    }

    private void BuyConsumable()
    {
        if (consumableData != null)
        {
            //Debug.Log($"{consumableData.itemName} 구매 시도!");
            //InventoryManager.Instance.UpdateUI();
            //// 1. 골드 차감 시도 (UseGole이 true면 충분, false면 부족)
            //bool goldUsed = InventoryManager.Instance.UseGole(consumableData.price);

            //if (!goldUsed)
            //{
            //    Debug.LogWarning($"골드 부족! {consumableData.itemName} 구매실패!");
            //    return;
            //}

            // 2. 아이템 인벤토리 추가
            //bool success = InventoryManager.Instance.TryAddItem(ItemManager.Instance.CreateItem(consumableData.id), 1);
            //if (success)
            //    Debug.Log($"{consumableData.itemName} 아이템 인벤토리에 추가됨!");
            //else
            //    Debug.LogWarning($"인벤토리 공간 부족! {consumableData.itemName} 구매실패!");

            //InventoryManager.Instance.UpdateUI();
        }
    }

    private void BuyWeapon()
    {
        if (weaponData != null)
        {
            Debug.Log($"{weaponData.weaponType} 구매 시도!");

            // 1. 골드 차감 시도
            //bool goldUsed = InventoryManager.Instance.UseGole(500);
      
            //if (!goldUsed)
            //{
            //    Debug.LogWarning($"골드 부족! {weaponData.weaponType} 구매실패!");
            //    return;
            //}

            //// 2. 무기 인벤토리 추가
            //bool success = InventoryManager.Instance.TryAddItem(ItemManager.Instance.CreateWeapon(weaponData.id), 1);
            //if (success)
            //    Debug.Log($"{weaponData.weaponType} 아이템 인벤토리에 추가됨!");
            //else
            //    Debug.LogWarning($"인벤토리 공간 부족! {weaponData.weaponType} 구매실패!");

            //InventoryManager.Instance.UpdateUI();
        }
    }
}
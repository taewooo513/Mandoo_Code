using DataTable; // GoogleSheet 자동 테이블 네임스페이스
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShopItemManager : MonoBehaviour
{
    public List<ConsumableData> consumableShopList = new List<ConsumableData>();
    public List<WeaponData> weaponShopList = new List<WeaponData>();

    public GameObject shopItemPrefab; // ShopItem 프리팹 (버튼, 아이콘, 텍스트 등)
    public Transform itemButtonParent; // 상점 UI에서 버튼들이 들어갈 부모

    void Start()
    {
        // ConsumableData 리스트 사용
        consumableShopList.AddRange( ConsumableData.GetList().Where(data => data.id != 1001));

        // WeaponData 리스트 사용, 숙련도 1만 33% 확률로 진열
        foreach (var w in WeaponData.GetList())
        {
            if (w.proficiencyLevel == 1) // 숙련도 1만!
            {
                float rand = UnityEngine.Random.Range(0f, 1f);
                if (rand < 0.33f)
                {
                    weaponShopList.Add(w);
                }
            }
        }

        // UI에 ShopItem 프리팹 동적으로 생성
        CreateShopItems();
    }

    void CreateShopItems()
    {
        foreach (var c in consumableShopList)
        {
            GameObject obj = Instantiate(shopItemPrefab, itemButtonParent);
            var shopItem = obj.GetComponent<ShopItem>();
            shopItem.SetConsumableData(c); // 데이터 전달
        }
        foreach (var w in weaponShopList)
        {
            GameObject obj = Instantiate(shopItemPrefab, itemButtonParent);
            var shopItem = obj.GetComponent<ShopItem>();
            shopItem.SetWeaponData(w); // 데이터 전달
        }
    }
}
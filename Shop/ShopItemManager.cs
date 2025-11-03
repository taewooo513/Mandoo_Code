using System.Collections.Generic;
using UnityEngine;
using DataTable;

public class ShopItemManager : MonoBehaviour
{
    public GameObject shopItemPrefab;
    public Transform itemButtonParent;

    // 등장확률(dropProb) 적용하여 아이템 선정
    public List<StoreData> GetRandomStoreDataByDropProb()
    {

        var tempList = StoreData.GetList();
        var storeDataList = new List<StoreData>();

        foreach (var item in tempList)
        {
            if (item.dropProb >= 1.0f || item.dropProb >= Random.value)
            {
                storeDataList.Add(item);
            }
        }
        return storeDataList;
    }

    public void OpenShop()
    {
        var filteredStoreData = GetRandomStoreDataByDropProb();
        CreateShopItems(filteredStoreData);
    }

    void CreateShopItems(List<StoreData> storeDataList)
    {
        foreach (var storeData in storeDataList)
        {
            GameObject obj = Instantiate(shopItemPrefab, itemButtonParent);
            var shopItem = obj.GetComponent<ShopItem>();
            shopItem.SetStoreData(storeData); // 데이터 전달
        }
    }
}
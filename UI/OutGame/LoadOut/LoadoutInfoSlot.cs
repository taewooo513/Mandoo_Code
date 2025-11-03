using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadoutInfoSlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private TextMeshProUGUI desc;

    public void SetInfoSlot(OutGameItem loadoutItem)
    {
        var path = loadoutItem.loadoutItemPath;
        var sprite = Resources.Load<Sprite>(path);
        var nameText = loadoutItem.name;
        var descText = loadoutItem.description;
        
        icon.sprite = sprite;
        name.text = nameText;
        desc.text = descText;
    }
    
    public void HideInfoSlot()
    {
        gameObject.SetActive(false);
    }
    
    public void ShowInfoSlot()
    {
        gameObject.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class InGameEquipmentUI : MonoBehaviour, ItemDragDropManager
{
    PlayableCharacter playableCharacter;

    InGameItem[] items;
    Skill swapSkill;
    public InGameItem[] GetItems => items;
    [SerializeField]
    private ItemDrag[] itemDrag;
    [SerializeField]
    private GameObject[] levelIcon;

    [SerializeField]
    private ItemDrop[] itemDrop;

    public void Awake()
    {
        swapSkill = new Skill();
        swapSkill.Init(500);
    }
    public void RemoveItem(InGameItem item)
    {

    }
    public void Setting(InGameItem[] items, PlayableCharacter playable)
    {
        this.items = items;
        playableCharacter = playable;
        swapSkill.baseEntity = playable;
        UpdateUI();
    }
    public void OnClickSkipButton()
    {
        if (playableCharacter != null && BattleManager.Instance.isBattleStarted)
        {
            if (BattleManager.Instance.isEndTurn)
            {
                BattleManager.Instance.EndTurn();
                UIManager.Instance.OpenUI<InGameUIManager>().OnClickSkillButton(null);
                BattleManager.Instance.isEndTurn = false;
            }
        }
    }

    public void OnClickChangeButton()
    {
        if (BattleManager.Instance.isEndTurn && BattleManager.Instance.isBattleStarted)
        {
            if (swapSkill.baseEntity != null)
                UIManager.Instance.OpenUI<InGameUIManager>().OnClickSkillButton(swapSkill);
        }
    }

    public void UpdateUI()
    {
        if (itemDrop[0] != null)
        {
            if (items[0] != null)
            {
                if (((Weapon)items[0]).proficiencyLevel == 1)
                {
                    levelIcon[0].SetActive(false);
                    levelIcon[1].SetActive(false);
                }
                else if (((Weapon)items[0]).proficiencyLevel == 2)
                {
                    levelIcon[0].SetActive(true);
                    levelIcon[1].SetActive(false);
                }
                else if (((Weapon)items[0]).proficiencyLevel == 3)
                {
                    levelIcon[0].SetActive(false);
                    levelIcon[1].SetActive(true);
                }
            }
        }
        if (itemDrag != null)
        {
            for (int i = 0; i < itemDrag.Length; i++)
            {
                if (itemDrag[i] != null)
                {
                    itemDrop[i].UpdateUI(items[i]);
                }
            }
        }

    }
    public void UnEquip(bool isWeapon)
    {
        if (playableCharacter != null)
        {
            if (isWeapon == true)
            {
                playableCharacter.entityInfo.skills[3] = null;
            }
            UIManager.Instance.OpenUI<InGamePlayerUI>().UpdateUI(playableCharacter.entityInfo, playableCharacter.entityInfo.skills, playableCharacter);
        }
    }
    public void OnDropSwap(int index1, int index2)
    {
    }
    public void OnDrop()
    {
    }
    public bool OnDrop(ItemDrop drop, IDraggingObject draggingObject)
    {
        if (playableCharacter == null) return false;

        if (draggingObject.item.GetItemInfo.type != ItemType.Equipment) return false;

        if (draggingObject.item == null) return false;

        switch (draggingObject.Origin)
        {
            case DragOrigin.Inventory:
                if (items[drop.SlotIndex] == null)
                {
                    if (drop.SlotIndex == 0)
                    {
                        if (((Weapon)draggingObject.item).type != WeaponType.Accessory)
                        {
                            InGameItemManager.Instance.GetItems()[draggingObject.SlotIndex] =
                                playableCharacter.EquipWeapon((Weapon)draggingObject.item);
                        }
                    }
                    else
                    {
                        if (((Weapon)draggingObject.item).type == WeaponType.Accessory)
                        {
                            InGameItemManager.Instance.GetItems()[draggingObject.SlotIndex] =
                                playableCharacter.EquipAcc((Weapon)draggingObject.item, drop.SlotIndex);
                        }
                    }
                }
                playableCharacter.UpdateUI();
                draggingObject.DraggingManager.UpdateUI();
                UpdateUI();
                return true;
            case DragOrigin.Reward:
                if (items[drop.SlotIndex] == null)
                {
                    if (drop.SlotIndex == 0)
                    {
                        if (((Weapon)draggingObject.item).type != WeaponType.Accessory)
                        {
                            draggingObject.DraggingManager.GetItems[draggingObject.SlotIndex] = null;
                        }
                    }
                    else
                    {
                        if (((Weapon)draggingObject.item).type == WeaponType.Accessory)
                        {
                            draggingObject.DraggingManager.GetItems[draggingObject.SlotIndex] = null;
                        }
                    }
                }
                playableCharacter.UpdateUI();
                UpdateUI();
                return true;
            case DragOrigin.Equipment:
                if (drop.SlotIndex != draggingObject.SlotIndex && drop.SlotIndex != 0 && draggingObject.SlotIndex != 0)
                {
                    (items[2], items[1]) = (items[1], items[2]);
                }
                else
                {
                    return false;
                }
                playableCharacter.UpdateUI();
                UpdateUI();

                return true;
            default:
                return false;
        }
    }

    public void AccWeaponEquipItem(ItemDrop drop, IDraggingObject draggingObject) // 악세서리랑 웨폰 확인후 장착하는부분
    {
    }
}

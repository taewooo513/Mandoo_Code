using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    private BaseRoom _room;
    [SerializeField] private Button button;
    private Outline _outline;
    public void Init(BaseRoom room)
    {
        _room = room;
        button.gameObject.SetActive(false);
        SetIcon();
        _room.SetRoomUI(this);
        _outline = GetComponentInChildren<Outline>();
        _outline.enabled = false;
    }

    public void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void SetIcon()
    {
        switch (_room)
        {
            case BattleRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Trap");
                //icon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
                break;
            case EmptyRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Empty");
                //icon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
                break;
            case TreasureRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Treasure");
                //icon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
                break;
            case PmcRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/PMC");
                //icon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
                break;
            case ShopRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Shop");
                //icon.gameObject.SetActive(false);
                icon.gameObject.SetActive(true);
                break;
            case StartRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Start");
                icon.gameObject.SetActive(true);
                break;
            case VillageRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Village");
                icon.gameObject.SetActive(true);
                break;
        }
    }

    public void ActivateIcon()
    {
        icon.gameObject.SetActive(true);
    }

    public void ActivateButton()
    {
        _outline.enabled = true;
        button.gameObject.SetActive(true);
    }

    public void EnterCorridor()
    {
        //_room.EnterCorridor(_room);
    }

    public void OnButtonClick()
    {
        //_room.ExitRoom();
        MapManager.Instance.CurrentLocation.Travel(_room);
        Time.timeScale = 1f;
        
    }

    public void DeactivateButton()
    {
        _outline.enabled = false;
        button.gameObject.SetActive(false);
    }
}

using System;
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
    private RectTransform _rectTransform;
    private bool _isIncreasing = true;
    public void Init(BaseRoom room)
    {
        _room = room;
        button.gameObject.SetActive(false);
        SetIcon();
        _room.SetRoomUI(this);
        _outline = GetComponentInChildren<Outline>();
        _rectTransform = GetComponent<RectTransform>();
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
                icon.gameObject.SetActive(false);
                break;
            case EmptyRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Empty");
                icon.gameObject.SetActive(false);
                break;
            case TreasureRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Treasure");
                icon.gameObject.SetActive(false);
                break;
            case PmcRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/PMC");
                icon.gameObject.SetActive(false);
                break;
            case ShopRoom:
                icon.sprite = Resources.Load<Sprite>("Sprites/Icon/Map/Shop");
                icon.gameObject.SetActive(false);
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
        if(GameManager.Instance.CurrentMapIndex == 0)
        {
            switch (AnalyticsManager.Instance.Step)
            {
                case 4:
                    AnalyticsManager.Instance.SendEventStep(5);
                    break;
                case 16:
                    AnalyticsManager.Instance.SendEventStep(17);
                    break;
                case 22:
                    AnalyticsManager.Instance.SendEventStep(23);
                    break;
            }
        }
  
        MapManager.Instance.CurrentLocation.Travel(_room);
        GameManager.Instance.playerCanMove = true;
        //UIManager.Instance.OpenUI<UseWeaponTutorial>();
        Tutorials.ShowIfNeeded<UseWeaponTutorial>();
    }

    public void DeactivateButton()
    {
        _outline.enabled = false;
        _rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        button.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_outline.enabled)
        {
            ChangeScale();
        }
    }

    private void ChangeScale()
    {
        if (_isIncreasing)
        {
            _rectTransform.localScale += new Vector3(0.2f, 0.2f, 0.2f)*Time.deltaTime;
            if (_rectTransform.localScale.x >= 1.2f)
            {
                _rectTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                _isIncreasing = false;
            }
        }
        else
        {
            _rectTransform.localScale -= new Vector3(0.2f, 0.2f, 0.2f)*Time.deltaTime;
            if (_rectTransform.localScale.x <= 1.0f)
            {
                _rectTransform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                _isIncreasing = true;
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapUI : UIBase
{
    [SerializeField] private Button zoomButton;
    [SerializeField] private Image zoomInImage;
    [SerializeField] private Image zoomOutImage;
    private bool _isZoomIn = true;
    private MapUIContentGenerator _mapUIContentGenerator;
    private List<BaseRoom> _rooms;
    [SerializeField] private Transform content;
    private RectTransform _contentRect;
    private List<RoomUI> _roomUIs = new();
    private List<CorridorUI> _corridorUIs = new();
    private void Awake()
    {
        _mapUIContentGenerator = gameObject.GetComponentInChildren<MapUIContentGenerator>();
        _contentRect = content.GetComponent<RectTransform>();
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        zoomButton.onClick.RemoveAllListeners();
        zoomButton.onClick.AddListener(OnClickButton);
        UIManager.Instance.RaiseUIVisibilityChanged(this, true);
    }

    protected override void OnClose()
    {
        base.OnClose();
        zoomButton.onClick.RemoveAllListeners();
        UIManager.Instance.RaiseUIVisibilityChanged(this, false);
    }

    private void Start()
    {
        
    }

    public void Init(List<BaseRoom> rooms)
    {
        _rooms = rooms;
    }

    public void GenerateMapUI()
    {
        ClearMapUI();
        _mapUIContentGenerator.Init(content, _rooms);
        _mapUIContentGenerator.GenerateMapUI(out _roomUIs, out _corridorUIs);
    }

    private void ClearMapUI()
    {
        foreach (var item in _roomUIs)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in _corridorUIs)
        {
            Destroy(item.gameObject);
        }
        _roomUIs.Clear();
        _corridorUIs.Clear();
    }

    public void ActivateEveryRoomIcon()
    {
        foreach(var item in _roomUIs)
        {
            item.ActivateIcon();
        }
    }

    private void OnClickButton()
    {
        Debug.Log($"ButtonClicked");
        if (_isZoomIn)
        {
            _contentRect.localScale = new Vector2(1.0f, 1.0f);
            zoomInImage.gameObject.SetActive(false);
            zoomOutImage.gameObject.SetActive(true);
            _isZoomIn = false;
        }
        else
        {
            _contentRect.localScale = new Vector2(0.5f, 0.5f);
            zoomInImage.gameObject.SetActive(true);
            zoomOutImage.gameObject.SetActive(false);
            _isZoomIn = true;
        }
    }
}

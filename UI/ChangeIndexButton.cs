using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ChangeIndexButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltip;
    private Tooltip tooltipScript;
    
    public event Action OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    private void Awake()
    {
        if (tooltip != null)
        {
            tooltipScript = tooltip.GetComponent<Tooltip>();
            tooltipScript.HideTooltip();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnPointerEnterEvent?.Invoke();
        string name = "위치변경";
        string desc = "[위치변경]\n<color=yellow>●●●●    ●●●●</color>\n단일 선택\n위치 변경";
        if (tooltipScript != null)
        {
            tooltipScript.SetTooltipText(name, desc);
            tooltipScript.ShowTooltip();
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        OnPointerExitEvent?.Invoke();
        tooltipScript.HideTooltip();
    }
}

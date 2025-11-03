using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class SkipTurnButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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
        string name = "<color=red>턴 종료 버튼</color>";
        string desc = "<color=red>현재 캐릭터의 턴을 종료합니다.\n종료 시 다음 캐릭터가 행동합니다.</color>";
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

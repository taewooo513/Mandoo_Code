using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class SelectSkillButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{

    private Button skillButton;
    private InGameUIManager inGameUIManager;
    private Image image;
    private Skill skill;
    [SerializeField] private GameObject tooltip;
    private Tooltip tooltipScript;
    private bool isHovered;
    private bool isActive;
    
    public event Action OnPointerEnterEvent;
    public event Action OnPointerExitEvent;

    private void Awake()
    {
        skillButton = GetComponent<Button>();
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
        image = GetComponent<Image>();

        if (tooltip != null)
        {
            tooltipScript = tooltip.GetComponent<Tooltip>();
            tooltipScript.HideTooltip();
        }

        isHovered = false;
        isActive = false;
    }

    private void OnEnable()
    {
        BattleManager.Instance.OnTurnEnded += ForceClose;
    }
    
    private void OnDisable()
    {
        BattleManager.Instance.OnTurnEnded -= ForceClose;
    }

    private void ForceClose()
    {
        isHovered = false;
        tooltipScript.HideTooltip();
        OnPointerExitEvent?.Invoke();
    }
    
    public void SetButton(Skill skill)
    {
        if (skill == null)
        {
            this.skill = null;
            image.sprite = Resources.Load<Sprite>("Sprites/Icon/Panels");
            skillButton.onClick.RemoveAllListeners();
            image.color = new Color(1, 1, 1);
            isActive = false;
            UpdateTooltip();
            return;
        }
        if (skill.IsAblePosition() == false)
        {
            skillButton.onClick.RemoveAllListeners();
            image.sprite = Resources.Load<Sprite>(skill.skillInfo.iconPathString);
            image.color = new Color(50f / 255f, 50f / 255f, 50f / 255f);
            this.skill = skill;
            isActive = false;
            UpdateTooltip();
            return;
        }
        image.color = new Color(1, 1, 1);
        skillButton.onClick.RemoveAllListeners();
        skillButton.onClick.AddListener(() => OnClickSkillButton(skill));
        image.sprite = Resources.Load<Sprite>(skill.skillInfo.iconPathString);

        this.skill = skill;
        isActive = true;
        UpdateTooltip();
    }

    public void OnDrag(PointerEventData eventData)
    {
        tooltipScript.HideTooltip();
    }
    
    private void OnClickSkillButton(Skill skill)
    {
        if (BattleManager.Instance.isEndTurn == true)
        {
            inGameUIManager.OnClickSkillButton(skill);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
        OnPointerEnterEvent?.Invoke();
        UpdateTooltip();
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
        OnPointerExitEvent?.Invoke();
        if (tooltipScript != null)
            tooltipScript.HideTooltip();

    }
    
    private void UpdateTooltip()
    {
        if (isHovered)
        {
            if (skill != null && skill.skillInfo != null)
            {
                string skillName = skill.skillInfo.skillName;
                string skillDesc = skill.skillInfo.description;
                if (tooltipScript != null)
                {
                    tooltipScript.SetTooltipText(skillName, skillDesc);
                    tooltipScript.ShowTooltip();
                }
            }
            else
            {
                if (tooltipScript != null)
                {
                    tooltipScript.HideTooltip();
                }
            }
        }
    }
}

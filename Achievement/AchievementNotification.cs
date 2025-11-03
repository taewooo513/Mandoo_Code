using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class AchievementNotification : UIBase
{
    [SerializeField] private RectTransform rt;
    [SerializeField] private float introPosition;
    [SerializeField] private float outroPosition;
    [SerializeField] private float duration;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI desc;
    
    private Queue<Achievement> q = new();
    private bool isPopup;
    
    private void Awake()
    {
        if (icon == null)
            icon = GetComponentInChildren<Image>();
        if (desc == null)
            desc = GetComponentInChildren<TextMeshProUGUI>();
        if (rt == null)
            rt = GetComponent<RectTransform>();
    }

    private void SetNotification(Achievement achievement)
    {
        var path = achievement.achievementInfo.achieveImgPath;
        var sprite = Resources.Load<Sprite>(path);
        var desText = achievement.achievementInfo.description;
        
        if (icon == null)
        {
            Debug.LogError("Default achievement icon is null");
            return;
        }

        if (path == null)
        {
            Debug.LogError("Achievement icon path is null");
            return;
        }
        
        if (sprite == null)
        {
            Debug.LogError("Achievement Sprite is null");
            // return;
        }

        if (desText == null)
        {
            Debug.LogError("Description Text is null");
            return;
        }
        
        icon.sprite = sprite;
        desc.text = desText;
        rt.anchoredPosition = new Vector2(outroPosition, 0f);
        rt.SetAsLastSibling();
    }

    public void Popup(Achievement achievement)
    {
        q.Enqueue(achievement);
        
        if (!isPopup)
        {
            SetNotification(achievement);
            StartCoroutine(PopupCoroutine());
            isPopup = true;
        }
    }
    
    private IEnumerator PopupCoroutine()
    {
        while (q.Count > 0)
        {
            var achievement = q.Dequeue();
            SetNotification(achievement);
            Intro();
            yield return new WaitForSeconds(duration + 1.5f);
            Outro();
            yield return new WaitForSeconds(duration);
        }

        isPopup = false;
    }
    
    private void Intro() => rt.DOAnchorPosX(introPosition, duration).SetEase(Ease.InQuad);
    
    private void Outro() => rt.DOAnchorPosX(outroPosition, duration).SetEase(Ease.InQuad);
}

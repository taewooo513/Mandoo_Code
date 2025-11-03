using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AchievementUISlot : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private TextMeshProUGUI maxCountText;
    [SerializeField] private GameObject fadeImage;
    public void Init(Achievement achievement)
    {
        icon.sprite = Resources.Load<Sprite>(achievement.achievementInfo.achieveImgPath);
        nameText.text = achievement.achievementInfo.name;
        descriptionText.text = achievement.achievementInfo.description;
        countText.text = AchievementManager.Instance.GetParam(achievement.achievementInfo.param).ToString();
        maxCountText.text = achievement.achievementInfo.requirementsValue.ToString();
        if (achievement.achievementInfo.isComplete)
        {
            fadeImage.SetActive(false);
        }
        else
        {
            fadeImage.SetActive(true);
        }
        
    }
}

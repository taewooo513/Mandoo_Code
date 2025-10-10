using UnityEngine;
using UnityEngine.EventSystems;

public class EnemyButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject targetImage;
    private InGameUIManager inGameUIManager;

    private void Awake()
    {
        inGameUIManager = UIManager.Instance.OpenUI<InGameUIManager>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (inGameUIManager.isSkillSelected && targetImage != null)

        targetImage.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetImage != null)
            targetImage.SetActive(false);
    }
}

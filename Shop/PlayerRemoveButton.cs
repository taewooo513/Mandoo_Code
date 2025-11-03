using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRemoveButton : MonoBehaviour
{
    public int spawnIndex; // 이 버튼이 해당하는 자리의 인덱스 (0~3)
    public Button button;
    private InGamePMCUI _inGamePMCUI;
    void Awake()
    {
        button.onClick.AddListener(() => {
            if (PMCHire.Instance != null)
                PMCHire.Instance.RemovePlayerAt(spawnIndex);
            _inGamePMCUI.RefreshCardsOnPanel();
        });
    }

    public void Init(InGamePMCUI inGamePMCUI)
    {
        _inGamePMCUI = inGamePMCUI;
    }
}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StopWPTutorial : Tutorials
{
    [SerializeField] private Image imageToToggle;      // 추가: 보여줄 이미지 참조 (Inspector에 드래그)
    [SerializeField] private TMP_Text messageText;     // TextMeshPro 사용
    [SerializeField] private KeyCode key1 = KeyCode.A;
    [SerializeField] private KeyCode key2 = KeyCode.D;
    [SerializeField] private string msgBeforeEquip = "무기를 장착해주세요";
    [SerializeField] private float showDuration = 2f;

    private Coroutine showingCoroutine;

    private void Start()
    {
        // 초기 상태: 이미지 숨기고 텍스트 비움
        if (imageToToggle != null)
            imageToToggle.gameObject.SetActive(false);
        if (messageText != null)
            messageText.text = "";
    }

    private void Update()
    {
        if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2))
        {
            ShowMessageWithImage(msgBeforeEquip);
        }
    }

    private void ShowMessageWithImage(string msg)
    {
        if (showingCoroutine != null)
            StopCoroutine(showingCoroutine);
        showingCoroutine = StartCoroutine(ShowForSeconds(msg, showDuration));
    }

    private IEnumerator ShowForSeconds(string msg, float seconds)
    {
        // 텍스트/이미지 표시
        if (messageText != null)
            messageText.text = msg;
        if (imageToToggle != null)
            imageToToggle.gameObject.SetActive(true);

        // 일시정지 상태에서도 보이게 하려면 WaitForSecondsRealtime를 사용하세요.
        yield return new WaitForSeconds(seconds);

        // 표시 해제
        if (imageToToggle != null)
            imageToToggle.gameObject.SetActive(false);
        if (messageText != null)
            messageText.text = "";

        showingCoroutine = null;
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    [SerializeField] private Button button;

    private void Start()
    {
        button.onClick.AddListener(OnClickButton);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }

    private void OnClickButton()
    {
        GameManager.Instance.StartGame();
    }
}

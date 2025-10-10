using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestartGame : UIBase
{
    [SerializeField] private Button button;
    
    private void OnClick()
    {
        SceneManager.LoadScene("1.Scenes/IntroScene");
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }
}

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
        SceneLoadManager.Instance.LoadScene(SceneKey.titleScene);
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }
}

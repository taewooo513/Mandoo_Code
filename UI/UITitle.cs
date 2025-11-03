using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITitle : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.OpenUI<MainTitleUI>();
    }
}

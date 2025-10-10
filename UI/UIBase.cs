using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBase : MonoBehaviour
{
    public void OpenUI()
    {
        gameObject.SetActive(true);
        OnOpen();
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
        OnClose();
    }

    protected virtual void OnOpen()
    {

    }

    protected virtual void OnClose()
    {

    }
}

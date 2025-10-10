using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconFactory
{
    public static string GetBuffIconPath(BuffInfo buff)
    {
        string path = "";
        switch (buff.buffType)
        {
            case BuffType.AttackUp:
                path = "AttackUp";
                break;
            case BuffType.AllStatUp:
                path = "AllStatUp";
                break;
            case BuffType.EvasionUp:
                path = "EvasionUp";
                break;
            case BuffType.CriticalUp:
                path = "CriticalUp";
                break;
            case BuffType.SpeedUp:
                path = "SpeedUp";
                break;
        }
        switch (buff.deBuffType)
        {
            case DeBuffType.AttackDown:
                path = "AttackDown";
                break;
            case DeBuffType.AllStatDown:
                path = "AllStatDown";
                break;
            case DeBuffType.EvasionDown:
                path = "EvasionDown";
                break;
            case DeBuffType.CriticalDown:
                path = "CriticalDown";
                break;
            case DeBuffType.SpeedDown:
                path = "SpeedDown";
                break;
            case DeBuffType.Damaged:
                path = "BurnIcon";
                break;
            case DeBuffType.Stun:
                path = "Stun";
                break;
        }
        return path;
    }
}

public class BuffIcon : MonoBehaviour
{
    private BuffInfo buff;
    Image image;
    Coroutine coroutine;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void UpdateUI(BuffInfo buff)
    {
        UnityEngine.Color color = image.color;
        color.a = 1;
        image.color = color;
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }

        this.buff = buff;
        image.sprite = SpriteManager.Instance.FindSprite(Constants.BuffSpriteIcon + BuffIconFactory.GetBuffIconPath(buff));

        if (buff.duration <= 1)
        {
            if (coroutine == null)
            {
                coroutine = StartCoroutine("BeforeEraseBuff");
            }
        }
    }
    private void OnDisable()
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    IEnumerator BeforeEraseBuff()
    {
        bool isFade = false;
        UnityEngine.Color color = image.color;
        while (true)
        {
            if (isFade)
            {
                if (color.a <= 0.3f)
                {
                    isFade = !isFade;
                }
                color.a -= 0.001f;
            }
            else
            {
                if (color.a >= 1)
                {
                    isFade = !isFade;
                    yield return new WaitForSeconds(0.1f);
                }
                color.a += 0.001f;
            }
            image.color = color;
            yield return null;
        }
    }
}

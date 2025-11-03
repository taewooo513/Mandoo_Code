using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ExtraTurnText : MonoBehaviour
{
    private bool _isTransformSet;
    private RectTransform _rectTransform;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        _rectTransform = text.GetComponent<RectTransform>();
    }

    public void OnExtraTurn()
    {
        text.gameObject.SetActive(true);
        _rectTransform.anchoredPosition = new Vector2(0, 20);
        _isTransformSet = true;
    }

    private void Update()
    {
        if (!_isTransformSet) return;
        _rectTransform.anchoredPosition += new Vector2(0, 15 * Time.deltaTime*0.3f);
        text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - Time.deltaTime*0.3f);

        if (text.color.a <= 0.0f)
        {
            _isTransformSet = false;
            text.color = new Color(text.color.r, text.color.g, text.color.b, 1.0f);
            text.gameObject.SetActive(false);
        }
    }
}

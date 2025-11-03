using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI DescText;
    
    // private float minX = -650f;
    // private float maxX = 650f;
    // private float minY = -350f;
    // private float maxY = 350f;

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }
    
    // public void SetTooltipPosition(Vector3 position)
    // {
    //     float clampedX = Mathf.Clamp(position.x, minX, maxX);
    //     float clampedY = Mathf.Clamp(position.y, minY, maxY);
    //     Vector3 newPosition = new Vector3(clampedX, clampedY, position.z);
    //     transform.position = newPosition;
    // }

    public void SetTooltipText(string name, string desc)
    {
        nameText.text = name;
        DescText.text = desc;
    }

    public void SetSortingOrder()
    {
        transform.SetAsLastSibling();
    }
    
    public void HideTooltip()
    {
        gameObject.SetActive(false);
    }
}

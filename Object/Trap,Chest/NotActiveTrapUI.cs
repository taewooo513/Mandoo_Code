using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotActiveTrapUI : MonoBehaviour
{
    public Button notActiveTrapButton;
    [SerializeField] private RangeCheck _rangeCheck;
    public GameObject outline;
    
    public void Start()
    {
        _rangeCheck = GetComponentInParent<RangeCheck>();
    }

    public void CatchTrapNotClick() //트랩이 발동했을 때, 트랩 다시 못 누르도록 하기
    {
        if (_rangeCheck != null) //범위 체크하는 부분이 있으면
        {
            _rangeCheck.Init(this);
            _rangeCheck.uiIsInteract = true; //다시 상호작용 안 되게끔 값변경
        }
    }
}
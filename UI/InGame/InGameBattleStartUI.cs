using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameBattleStartUI : UIBase
{
    [SerializeField]
    private float closeTimer;
    //public FadeInOut fadeInOut;

    // public void Start()
    // {
    //     fadeInOut = GetComponent<FadeInOut>();
    // }

    protected override void OnOpen()
    {
        //fadeInOut.FadeIn();
        base.OnOpen();
        StartCoroutine(ClosePanel());
   
    }

    private IEnumerator ClosePanel()
    {
        yield return new WaitForSeconds(closeTimer);

        // if (fadeInOut != null)
        // {
        //     yield return StartCoroutine(fadeInOut.FadeOutCoroutine());
        // }
        BattleManager.Instance.StartTurn();
        base.CloseUI();
    }
}

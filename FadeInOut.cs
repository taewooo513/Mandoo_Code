using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
 
public class FadeInOut : UIBase
{
    public CanvasGroup canvasGroup;
 
    public Tween FadeIn(float duration) //원하는 시간만큼 페이드 인 사용
    {
        this.gameObject.SetActive(true);
        canvasGroup.alpha = 1f;
        return canvasGroup.DOFade(0f, duration).SetEase(Ease.OutQuad) //알파값을 1에서 0으로 변경
            .OnComplete(() => this.gameObject.SetActive(false)); //페이드인 한 다음에 ui창 꺼줌
    }
 
    public Tween FadeOut(float duration) //페이드 아웃 사용
    {
        canvasGroup.alpha = 0f;
        return canvasGroup.DOFade(1f, duration).SetEase(Ease.OutQuad); //알파값 0에서 1로 변경
    }
}
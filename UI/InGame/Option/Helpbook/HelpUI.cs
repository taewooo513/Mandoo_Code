using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HelpUI : UIBase
{
    public GameObject[] helpBooks; //0 = map, 1 = weapon, 4 = corridor, 7 = battle
    public GameObject[] leftBookmarks;
    public GameObject[] rightBookmarks;
    public Button[] nextButton; //0 = left, 1 = right
    public HelpBookAnimation helpBookAnimation;
    public Button closeButton;
    
    private int _currentIndex;
    private bool _isAnimating = false; //애니메이션 동작중인지
    private int _targetIndex;
    private string _passDirection; //넘기는 방향, "Left" or "Right"

    public void Start()
    {
        Init();
    }

    public void Init()
    {
        _currentIndex = 0;
        if (helpBooks == null) return;

        UpdateContent(_currentIndex);
        UpdateBookmark(_currentIndex);
        UpdateButtonColors();
    }

    public void OnClickClose()
    {
        helpBookAnimation.HelpBookAnimationClear();
        OnBookAnimationEnd();
        Init(); //다시 켰을 때 0페이지부터 시작하기 위해서
        UIManager.Instance.CloseUI<HelpUI>();
    }

    public void OnClickLeft() //왼쪽 화살표 클릭
    {
        if (_isAnimating || _currentIndex <= 0) return;

        _passDirection = "Left";
        _targetIndex = _currentIndex - 1;
        _isAnimating = true;
        helpBookAnimation.HelpBookAnimationStartLeft(); //애니메이션 동작
        SetButtonsInteractable(false);
    }

    public void OnClickRight()
    {
        if (_isAnimating || _currentIndex >= helpBooks.Length - 1) return;

        _passDirection = "Right";
        _targetIndex = _currentIndex + 1;
        _isAnimating = true;
        helpBookAnimation.HelpBookAnimationStartRight();
        SetButtonsInteractable(false);
    }

    public void OnBookAnimationEnd() //애니메이션 종료 후
    {
        _currentIndex = _targetIndex; //실제 페이지 전환
        
        UpdateContent(_currentIndex);
        UpdateBookmark(_currentIndex);
        UpdateButtonColors();
        
        _isAnimating = false;
        SetButtonsInteractable(true); //버튼 활성화
    }

    public void OnBookmarkClicked(int pageIndex)
    {
        if(_isAnimating || pageIndex == _currentIndex) return;
        
        _passDirection = pageIndex > _currentIndex ? "Right" : "Left";
        _targetIndex = pageIndex;
        
        //버튼 잠금 + 애니메이션 시작
        _isAnimating = true;
        SetButtonsInteractable(false);
        
        if (_passDirection == "Right")
            helpBookAnimation.HelpBookAnimationStartRight();
        else
            helpBookAnimation.HelpBookAnimationStartLeft();
    }

    public void OnClickBookmarkMap() //북마크 클릭
    {
        OnBookmarkClicked(0);
    }

    public void OnClickBookmarkWeapon()
    {
        OnBookmarkClicked(1);
    }

    public void OnClickBookmarkCorridor()
    {
        OnBookmarkClicked(4);
    }

    public void OnClickBookmarkBattle()
    {
        OnBookmarkClicked(7);
    }

    private void SetButtonsInteractable(bool isInteractable) //버튼 상호작용 막는 용도
    {
        foreach (var button in nextButton)
            button.interactable = isInteractable;
        foreach (var bookmark in leftBookmarks)
            bookmark.GetComponent<Button>().interactable = isInteractable;
        foreach (var bookmark in rightBookmarks)
            bookmark.GetComponent<Button>().interactable = isInteractable;
        closeButton.interactable = isInteractable;
    }

    private void UpdateButtonColors()
    {
        Image leftImage = nextButton[0].GetComponent<Image>();
        Image rightImage = nextButton[1].GetComponent<Image>();

        if (_currentIndex <= 0) //제일 왼쪽일 때
        {
            leftImage.color = Color.black;
            rightImage.color = Color.white;
        }
        else if (_currentIndex >= helpBooks.Length - 1) //제일 오른쪽일 때
        {
            leftImage.color = Color.white;
            rightImage.color = Color.black;
        }
        else //중간
        {
            leftImage.color = Color.white;
            rightImage.color = Color.white;
        }
    }
    
    private void UpdateContent(int curIndex)
    {
        for (int i = 0; i < helpBooks.Length; i++)
            helpBooks[i].SetActive(curIndex == i); //현재 index번호만 true
    }

    private void UpdateBookmark(int curIndex)
    {
        int bookmarkMap = 0;
        int bookmarkWeapon = 1;
        int bookmarkCorridor = 4;
        int bookmarkBattle = 7;

        if (helpBooks[curIndex] == helpBooks[bookmarkMap])
        {
            AllBookmarksOff();
            rightBookmarks[0].SetActive(false);
            for (int i = 1; i < rightBookmarks.Length; i++)
            {
                rightBookmarks[i].SetActive(true);
            }
        }
        else if (helpBooks[curIndex] == helpBooks[bookmarkWeapon])
        {
            AllBookmarksOff();
            int currentBookmarkIndex = 1;
            for (int i = 0; i < currentBookmarkIndex; i++)
            {
                leftBookmarks[i].SetActive(true);
            }
            rightBookmarks[currentBookmarkIndex].SetActive(false);
            for (int i = currentBookmarkIndex + 1; i < rightBookmarks.Length; i++)
            {
                rightBookmarks[i].SetActive(true);
            }
        }
        else if (helpBooks[curIndex] == helpBooks[bookmarkCorridor])
        {
            AllBookmarksOff();
            int currentBookmarkIndex = 2;
            for (int i = 0; i < currentBookmarkIndex; i++)
            {
                leftBookmarks[i].SetActive(true);
            }

            rightBookmarks[currentBookmarkIndex].SetActive(false);
            for (int i = currentBookmarkIndex + 1; i < rightBookmarks.Length; i++)
            {
                rightBookmarks[i].SetActive(true);
            }
        }
        else if (helpBooks[curIndex] == helpBooks[bookmarkBattle])
        {
            AllBookmarksOff();
            int currentBookmarkIndex = 3;
            for (int i = 0; i < currentBookmarkIndex; i++)
            {
                leftBookmarks[i].SetActive(true);
            }

            rightBookmarks[currentBookmarkIndex].SetActive(false);
            for (int i = currentBookmarkIndex + 1; i < rightBookmarks.Length; i++)
            {
                rightBookmarks[i].SetActive(true);
            }
        }
    }

    private void AllBookmarksOff()
    {
        for (int i = 0; i < leftBookmarks.Length; i++)
        {
            leftBookmarks[i].SetActive(false);
        }
        for (int i = 0; i < rightBookmarks.Length; i++)
        {
            rightBookmarks[i].SetActive(false);
        }
    }
}

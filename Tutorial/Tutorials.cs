using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorials : UIBase
{
    private static readonly HashSet<string> s_shownThisSession = new();
    protected virtual bool PersistAcrossSessions => false;
    protected virtual string TutorialKey => GetType().Name + "_shown";
    [SerializeField] protected string requiredSceneName = "";

    protected override void OnOpen()
    {
        // 씬 제한이 설정되어 있으면 현재 씬이 일치하는지 확인
        if (!string.IsNullOrEmpty(requiredSceneName))
        {
            var active = SceneManager.GetActiveScene();
            if (active.name != requiredSceneName)
            {
                // 현재 씬이 요구 씬이 아니면 열자마자 닫음
                CloseUI();
                GameManager.Instance.playerCanMove = true;
                return;
            }
        }

        // 이미 본 튜토리얼이면 자동으로 숨김
        if (IsShown())
        {
            CloseUI();
            GameManager.Instance.playerCanMove = true;
            return;
        }
        
        MarkShown();
        GameManager.Instance.playerCanMove = false;
    }

    protected override void OnClose()
    {
        MarkShown();
        // 필요시 종료 처리
    }

    private bool IsShown()
    {
        if (PersistAcrossSessions)
        {
            return PlayerPrefs.GetInt(TutorialKey, 0) == 1;
        }
        else
        {
            return s_shownThisSession.Contains(TutorialKey);
        }
    }

    protected void MarkShown()
    {
        if (PersistAcrossSessions)
        {
            PlayerPrefs.SetInt(TutorialKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            s_shownThisSession.Add(TutorialKey);
        }
    }

    public static void ShowIfNeeded<T>() where T : Tutorials
    {
        string key = typeof(T).Name + "_shown";

        // 영속적으로 저장된 기록이 있으면 열지 않음
        if (PlayerPrefs.GetInt(key, 0) == 1)
        {
            return;
        }

        // 세션내에 이미 표시된 경우 스킵
        if (s_shownThisSession.Contains(key))
        {
            return;
        }
        UIManager.Instance.OpenUI<T>();
    }

    //(런타임 중 강제 리셋용)
    public static void ResetSessionShown<T>() where T : Tutorials
    {
        string key = typeof(T).Name + "_shown";
        s_shownThisSession.Remove(key);
    }
    public static void ResetPersistedShown<T>() where T : Tutorials
    {
        string key = typeof(T).Name + "_shown";
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
    }
    public static void ResetAllSessionShown(bool clearPlayerPrefs = true)
    {
        s_shownThisSession.Clear(); // 세션 컬렉션 전체를 비움 (프로젝트 구조에 따라 안전성 확인)

        if (clearPlayerPrefs)
        {
            var tutorialTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch { return Type.EmptyTypes; }
                })
                .Where(t => t != null && !t.IsAbstract && typeof(Tutorials).IsAssignableFrom(t));

            foreach (var t in tutorialTypes)
            {
                string key = t.Name + "_shown";
                if (PlayerPrefs.HasKey(key)) PlayerPrefs.DeleteKey(key);
            }
            PlayerPrefs.Save();
        }
    }
    public void OnCloseButton()
    {
        CloseUI(); 
    }

    public void OnDontShowAgainButton()
    {
        PlayerPrefs.SetInt(TutorialKey, 1);
        PlayerPrefs.Save();

        // 세션에도 기록해 중복 방지
        s_shownThisSession.Add(TutorialKey);

        CloseUI();
        GameManager.Instance.playerCanMove = true;
    }

}
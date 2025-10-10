using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackgroundManager : Singleton<BackgroundManager>
{
    private RoomBackground _background;
    private SpriteRenderer _spriteRenderer;
    private CorridorBackground _corridorBackground;
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private float fadeOutTime = 2f;
    private void Start()
    {
    }
    
    public void ChangeBackground(INavigatable location) //배경 변경
    {
        if (location is BaseRoom room) //위치가 룸에 있을 때
        {
            if (_background == null) InstantiateBackgrounds();
            UIManager.Instance.OpenUI<FadeInOut>().FadeOut(fadeOutTime); //페이드아웃
            _background.gameObject.SetActive(true);
            _corridorBackground.gameObject.SetActive(false);
            _spriteRenderer.sprite = Resources.Load<Sprite>(room.GetBackgroundPath());
        }
        else
        {
            if (_corridorBackground == null) InstantiateBackgrounds();
            UIManager.Instance.OpenUI<FadeInOut>().FadeOut(fadeOutTime); //페이드아웃
            _corridorBackground.gameObject.SetActive(true);
            _background.gameObject.SetActive(false);
            _corridorBackground.gameObject.transform.position = new Vector3(19.8f, -0.5f, 0);
            _corridorBackground.Init();
        }
        UIManager.Instance.OpenUI<FadeInOut>().FadeIn(fadeInTime); //페이드인 해서 화면 밝게
    }

    private void InstantiateBackgrounds() //초기값 세팅
    {
        var go = Instantiate(Resources.Load<GameObject>("Prefabs/Map/RoomBackground"));
        _background = go.GetComponent<RoomBackground>();
        _spriteRenderer = go.GetComponentInChildren<SpriteRenderer>();
        var go2 = Instantiate(Resources.Load<GameObject>("Prefabs/Map/CorridorBackground"));
        _corridorBackground = go2.GetComponent<CorridorBackground>();
        _background.gameObject.SetActive(false);
        _corridorBackground.gameObject.SetActive(false);
    }
}

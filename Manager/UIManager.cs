using System.Collections;
using System;
using System.Collections.Generic;
using System.Resources;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class UIManager : Singleton<UIManager>
{
    private Transform _canvas;
    private EventSystem _eventSystem;

    private Dictionary<string, UIBase> _uiDictionary = new();

    private bool IsExistUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;
        return _uiDictionary.TryGetValue(uiName, out var ui) && ui != null;
    }

    public T OpenUI<T>() where T : UIBase
    {
        var ui = GetUI<T>();
        ui?.OpenUI();
        return ui;
    }

    public void CloseUI<T>() where T : UIBase
    {
        if (IsExistUI<T>())
        {
            var ui = GetUI<T>();
            ui?.CloseUI();
        }
    }

    private T GetUI<T>() where T : UIBase
    {
        var uiName = typeof(T).Name;
        UIBase ui;
        ui = IsExistUI<T>() ? _uiDictionary[uiName] : CreateUI<T>();
        return ui as T;
    }
    // UI 제거 (제네릭)
    public void RemoveUI<T>() where T : UIBase
    {
        // 1. 타입 이름을 키로 사용
        string key = typeof(T).Name;

        // 2. 해당 키가 딕셔너리에 있는지 확인
        if (_uiDictionary.ContainsKey(key))
        {
            // 3. UI 오브젝트(게임오브젝트)를 씬에서 제거
            Destroy(_uiDictionary[key].gameObject);

            // 4. 관리 딕셔너리에서 해당 UI 삭제
            _uiDictionary.Remove(key);
        }
    }

    private T CreateUI<T>() where T : UIBase
    {
        //1. _uiDictionary에 해당 UI 있는지 확인, 있으면 파괴 후 사전에서 삭제
        var uiName = typeof(T).Name;
        if (_uiDictionary.TryGetValue(uiName, out var ui) && ui != null)
        {
            Destroy(ui.gameObject);
            _uiDictionary.Remove(uiName);
        }
        //        //2. 캔버스, 이벤트 시스템 확인
        CheckCanvas();
        CheckEventSystem();

        // 3. 프리팹 로드 & 생성, 이후 프리팹이 생성되었는지 확인.
        var path = Constants.UIElementsPath + uiName;
        GameObject go = Resource.Instance.Create<GameObject>(path, _canvas);
        if (go == null)
        {
            Debug.LogError($"Prefab not found: {uiName}");
            return null;
        }
        // 4. 컴포넌트 획득 이후 컴포넌트가 들어갔는지 확인. 들어가지 않았다면 파괴
        T uiComponent = go.GetComponent<T>();
        if (uiComponent == null)
        {
            Debug.LogError($"Component not found: {uiName}");
            Destroy(go);
            return null;
        }

        // 5. Dictionary 등록
        //_uiDictionary.Add(uiName, uiComponent); <- 중복된 값이 들어갈 수 있어 위험함
        _uiDictionary[uiName] = uiComponent;
        return uiComponent;
    }
    public T CreateSlotUI<T>(Transform parent = null) where T : UIBase
    {
        string uiName = typeof(T).Name;
        string path = Constants.UIElementsPath + uiName;

        GameObject go = Resource.Instance.Create<GameObject>(path, parent);
        if (go == null)
        {
            Debug.LogError($"Prefab not found: {uiName}");
            return null;
        }

        T uiComponent = go.GetComponent<T>();
        if (uiComponent == null)
        {
            Debug.LogError($"Component not found: {uiName}");
            Destroy(go);
            return null;
        }

        _uiDictionary[uiName] = uiComponent;
        return uiComponent;
    }

    public T CreateBarUI<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        string path = Constants.UIElementsPath + uiName;

        CheckCanvas();
        CheckEventSystem();

        GameObject go = Resource.Instance.Create<GameObject>(path, _canvas);
        if (go == null)
        {
            Debug.LogError($"Prefab not found: {uiName}");
            return null;
        }

        T uiComponent = go.GetComponent<T>();
        if (uiComponent == null)
        {
            Debug.LogError($"Component not found: {uiName}");
            Destroy(go);
            return null;
        }

        _uiDictionary[uiName] = uiComponent;
        return uiComponent;
    }

    public T CreateUIDontDestroy<T>() where T : UIBase
    {
        string uiName = typeof(T).Name;
        string path = Constants.UIElementsPath + uiName;

        CheckCanvas();
        CheckEventSystem();

        GameObject go = Resource.Instance.Create<GameObject>(path, _canvas);
        if (go == null)
        {
            Debug.LogError($"Prefab not found: {uiName}");
            return null;
        }

        T uiComponent = go.GetComponent<T>();
        if (uiComponent == null)
        {
            Debug.LogError($"Component not found: {uiName}");
            Destroy(go);
            return null;
        }
        return uiComponent;
    }
    private void CheckCanvas()
    {
        //캔버스 있는지 확인, 있으면 return
        if (_canvas != null) return;
        //없으면 경로 만들고 생성 후 _canvas 초기화
        var path = Constants.UICommonPath + Constants.Canvas;
        _canvas = Resource.Instance.Create<Transform>(path, null);
        DontDestroyOnLoad(_canvas.gameObject);
    }

    private void CheckEventSystem()
    {
        //이벤트 시스템 있는지 확인, 있으면 return
        if (_eventSystem != null) return;
        //없으면 경로 만들고 생성 후 _eventSystem 초기화
        var path = Constants.UICommonPath + Constants.EventSystem;
        _eventSystem = Resource.Instance.Create<EventSystem>(path, null);
        DontDestroyOnLoad(_eventSystem.gameObject);
    }

    public void ClearUI()
    {
        foreach (var ui in _uiDictionary.Values)
        {
            if (ui != null)
            {
                ui.CloseUI();
                Destroy(ui.gameObject);
            }
        }
        _uiDictionary.Clear();
    }

}

// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.UI;
//
// public class MenuController : MonoBehaviour
// {
//     public Button[] buttons;
//     private int _currentIndex = 0;
//     private Outline _currentOutline;
//     public static bool CanControl = true;
//
//     void Start()
//     {
//         if (buttons == null || buttons.Length == 0)
//         {
//             Debug.LogWarning("buttons 배열이 비어 있습니다!");
//             return;
//         }
//
//         _currentIndex = Mathf.Clamp(_currentIndex, 0, buttons.Length - 1);
//         ActiveButtonsClear();
//     }
//
//     void Update()
//     {
//         if (buttons == null || buttons.Length == 0) return;
//         if (!CanControl) return;
//         
//         List<Button> activeButtons = GetActiveButtons(); //활성화된 버튼 리스트
//         
//         if (activeButtons.Count == 0)
//             return;
//
//         if (Input.GetKeyDown(KeyCode.UpArrow))
//         {
//             _currentIndex = (_currentIndex + activeButtons.Count - 1) % activeButtons.Count;
//             UpdateSelection(activeButtons);
//         }
//         else if (Input.GetKeyDown(KeyCode.DownArrow))
//         {
//             _currentIndex = (_currentIndex + 1) % activeButtons.Count;
//             UpdateSelection(activeButtons);
//         }
//
//         if (Input.GetKeyDown(KeyCode.Return)) //엔터
//         {
//             activeButtons[_currentIndex].onClick.Invoke();
//         }
//     }   
//
//     private List<Button> GetActiveButtons() //활성화된 버튼
//     {
//         List<Button> activeButtons = new List<Button>();
//         foreach (var btn in buttons)
//         {
//             if (btn != null && btn.gameObject.activeInHierarchy)
//                 activeButtons.Add(btn);
//         }
//         return activeButtons;
//     }
//     
//     private void ActiveButtonsClear()
//     {
//         List<Button> activeButtons = GetActiveButtons();
//         _currentIndex = Mathf.Clamp(_currentIndex, 0, activeButtons.Count - 1);
//         UpdateSelection(activeButtons);
//     }
//     
//     void UpdateSelection(List<Button> activeButtons)
//     {
//         if (activeButtons == null || activeButtons.Count == 0)
//             return;
//
//         if (_currentOutline != null) //이전 아웃라인 제거
//         {
//             _currentOutline.enabled = false;
//         }
//         
//         var selectedButton = activeButtons[_currentIndex];
//         EventSystem.current.SetSelectedGameObject(selectedButton.gameObject);
//         
//         //현재 버튼의 아웃라인 가져오거나 추가
//         _currentOutline = selectedButton.GetComponent<Outline>();
//         if (_currentOutline == null)
//             _currentOutline = selectedButton.gameObject.AddComponent<Outline>();
//
//         // Outline 설정
//         _currentOutline.enabled = true;
//         _currentOutline.effectColor = Color.yellow;
//         _currentOutline.effectDistance = new Vector2(10f, -10f);
//     }
// }

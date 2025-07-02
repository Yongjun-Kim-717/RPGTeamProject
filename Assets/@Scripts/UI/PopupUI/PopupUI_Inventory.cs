using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 인벤토리 UI 스크립트
//- 버튼 이벤트 연결
//- PlayerInventoryData의 정보를 바탕으로 UI에 생성
[RequireComponent(typeof(EventTrigger))]
public class PopupUI_Inventory : MonoBehaviour
{
    [SerializeField] GameObject _slot;
    [SerializeField] GameObject _equipmentViewPort;
    [SerializeField] GameObject _consumeViewPort;
    [SerializeField] GameObject _otherViewPort;
    [SerializeField] Button _exitButton;

    public event Action OnExitButtonClicked;

    private void Awake()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnEnable()
    {
        _equipmentViewPort.SetActive(true);
        _consumeViewPort.SetActive(false);
        _otherViewPort.SetActive(false);
    }

    void CreateSlot()
    { 
        
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}

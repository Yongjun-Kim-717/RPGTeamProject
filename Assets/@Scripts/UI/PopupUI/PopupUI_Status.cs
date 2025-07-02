using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class PopupUI_Status : MonoBehaviour
{
    public event Action OnExitButtonClicked;

    [SerializeField] TMP_Text _silverCoinText;
    [SerializeField] List<UI_StatusSlot> _statusList;
    [SerializeField] Button _exitButton;

    PlayerController _player;
    PlayerInventoryData _inventoryData;


    private void Awake()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _inventoryData.OnValueChanged += UpdateStatusUI;
    }

    private void OnDisable()
    {
        _inventoryData.OnValueChanged -= UpdateStatusUI;
    }

    void Initialize() {
        _player = PlayerManager.Instance.Player;
        _inventoryData = _player.PlayerInventoryData;
        _silverCoinText.text = _inventoryData.silverCoin.ToString();
        foreach (var slot in _statusList)
        {
            slot.Initialize(_player.PlayerData, _inventoryData);
        }
        _exitButton.onClick.AddListener(OnExitButtonClick);
     }

    private void UpdateStatusUI(Define.GoodsType type)
    {
        if(type == Define.GoodsType.SilverCoin)
            _silverCoinText.text = _inventoryData.silverCoin.ToString();
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }
}

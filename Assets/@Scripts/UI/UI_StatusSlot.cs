using extension;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_StatusSlot : MonoBehaviour
{
    public Button upgradeButton;

    [SerializeField] StatusSlotData _statusSlotData;
    [SerializeField] TMP_Text _statusNameText;
    [SerializeField] TMP_Text _currentStatusText;
    [SerializeField] TMP_Text _upgradeCostText;

    EventTrigger _eventTrigger;
    PlayerData _playerData;
    PlayerInventoryData _inventoryData;

    bool _isButtonDown = false;
    bool _isSilverCoinLack = false;

    float _time = 0;
    float _delay = 0.3f;

    private void Awake()
    {
        SetButtonEventTrigger();
    }
    void SetButtonEventTrigger()
    {
        _eventTrigger = upgradeButton.GetOrAddComponent<EventTrigger>();
        EventTrigger.Entry entryPointerDown = new EventTrigger.Entry();
        entryPointerDown.eventID = EventTriggerType.PointerDown;
        entryPointerDown.callback.AddListener((eventData) => 
        { 
            if (!_isSilverCoinLack)
            {
                _isButtonDown = true; UpgradeStatus();
            }
        });

        EventTrigger.Entry entryPointerUp = new EventTrigger.Entry();
        entryPointerUp.eventID = EventTriggerType.PointerUp;
        entryPointerUp.callback.AddListener((eventData) => { _isButtonDown = false; });

        _eventTrigger.triggers.Add(entryPointerDown);
        _eventTrigger.triggers.Add(entryPointerUp);
    }

    public void Initialize(PlayerData playerData, PlayerInventoryData inventoryData)
    {
        _playerData = playerData;
        _inventoryData = inventoryData;

        _upgradeCostText.text = _statusSlotData.upgradeCost.ToString();
        _currentStatusText.text = _statusSlotData.currentStatusCount.ToString();
    }


    private void Update()
    {
        if (_isButtonDown)
        {
            _time += Time.deltaTime;

            if (_time > _delay)
            {
                UpgradeStatus();
                _time = 0;
                _delay = Mathf.Max(_delay * 0.9f, 0.05f);
            }
        }
        else
            _delay = 0.3f;
    }

    //스탯 업그레이드
    //근데 업글 비용과 스탯 수?를 정해야 한다
    public void UpgradeStatus()
    {
        if (_inventoryData.ModifyGoods(Define.GoodsType.SilverCoin, -_statusSlotData.upgradeCost))
        {
            _statusSlotData.level++;
            _statusSlotData.upgradeCost += 10 * _statusSlotData.level;
            _statusSlotData.currentStatusCount += 5 + _statusSlotData.level;

            _upgradeCostText.text = _statusSlotData.upgradeCost.ToString();
            _currentStatusText.text = _statusSlotData.currentStatusCount.ToString();

            ApplyUpgrade(_statusSlotData.statusType, _statusSlotData.currentStatusCount);
            _isSilverCoinLack = false;
        }
        else
        {
            _isSilverCoinLack = true;
        }
    }

    void ApplyUpgrade(Define.StatusType type, int amount)
    {
        switch (type)
        {
            case Define.StatusType.Atk:
                _playerData.Atk = amount;
                break;
            case Define.StatusType.HP:
                _playerData.HP = amount;
                break;
            case Define.StatusType.HPRecoveryPerSec:
                _playerData.HPRecoveryPerSec = amount;
                break;
            case Define.StatusType.Def:
                _playerData.Def = amount;
                break;
        }
    }
}

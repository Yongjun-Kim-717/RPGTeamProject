using System;
using UnityEditor.Rendering;
using UnityEngine;

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{
    public event Action<Define.JourneyEventType> OnJourneyEvent;

    StageController _stageController;
    SpawnController _spawnController;
    DungeonController _dungeonController;
    PlayerData _playerData;

    Define.JourneyEventType _currentEventType;

    int stageLevel;

    public StageController StageController
    {
        get { return _stageController; }
    }

    public DungeonController DungeonController
    {
        get { return _dungeonController; }
    }

    protected override void Initialize()
    {
        _dungeonController = new GameObject("DungeonController").AddComponent<DungeonController>();
        _stageController = new GameObject("StageController").AddComponent<StageController>();
        _spawnController = new GameObject("SpawnController").AddComponent<SpawnController>();

        _playerData = PlayerManager.Instance.Player.PlayerData;
    }
    void Start()
    {
        PopupUIManager.Instance.ActivateJourneyInfo();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        PlayerManager.Instance.Player.OnPlayerCrossed += OnPlayerCross;
        _dungeonController.OnDungeonClear += () => PlayerManager.Instance.Player.GainJourneyExp(Define.JourneyType.Dungeon);
    }
    #endregion


    //플레이어가 구역을 지나면 호출될 함수
    void OnPlayerCross()
    {
        int rnd = 0;
        if(rnd < 25)
            rnd = (int)Define.JourneyEventType.Dungeon;
        else if(rnd < 50)
            rnd = (int)Define.JourneyEventType.TreasureBox;
        else if(rnd < 85)
            rnd = (int)Define.JourneyEventType.Merchant;
        else
            rnd = (int)Define.JourneyEventType.OtherObject;

        _currentEventType = (Define.JourneyEventType)rnd;
        OnJourneyEvent?.Invoke(_currentEventType);
    }

    public void Deactivate()
    {
        
    }
}

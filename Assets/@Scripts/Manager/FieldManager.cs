using System;
using UnityEditor.Rendering;
using UnityEngine;

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{
    public event Action<Define.JourneyEventType> OnJourneyEvent;

    SpawnController _spawnController;
    DungeonController _dungeonController;
    PlayerData _playerData;

    public DungeonController DungeonController
    {
        get { return _dungeonController; }
    }

    protected override void Initialize()
    {
        _dungeonController = new GameObject("DungeonController").AddComponent<DungeonController>();
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
    }
    #endregion


    //플레이어가 구역을 지나면 호출될 함수
    void OnPlayerCross()
    {
        float rnd= UnityEngine.Random.Range(0, 2);
        OnJourneyEvent?.Invoke((Define.JourneyEventType)rnd);
    }

    public void Deactivate()
    {
        
    }
}

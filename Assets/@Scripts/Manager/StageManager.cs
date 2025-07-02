using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : Singleton<StageManager>, IEventSubscriber, IDeactivateObject
{
    private List<StageInfo> _stageInfos;
    private string _stageName;
    private Define.StageActionStatus _stageActionStatus;
    private bool _isSpawnNamedMonster = false;

    public Define.StageActionStatus StageActionStatus
    {
        get { return _stageActionStatus; }
        set
        {
            _stageActionStatus = value;
            OnStageActionChanged?.Invoke(_stageActionStatus);
        }
    }

    public bool IsSpawnNamedMonster
    {
        get { return _isSpawnNamedMonster; }
        set 
        { 
            _isSpawnNamedMonster = value;
            if (_isSpawnNamedMonster == true)
                FieldManager.Instance.DungeonController.OnSpawnNamedMonster?.Invoke();
        }
    }

    public event Action<Define.StageActionStatus> OnStageActionChanged;

    protected override void Initialize()
    {
        base.Initialize();
        _stageInfos = new List<StageInfo>();
        _stageActionStatus = Define.StageActionStatus.NotChallenge;
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        FieldManager.Instance.DungeonController.OnSpawnableNamedMonster += ChangeStageActionStatusToChallenge;
        FieldManager.Instance.DungeonController.OnDungeonExit += ClearSetting;
    }
    #endregion
    #region IDeactivateObject
    public void Deactivate()
    {
        throw new System.NotImplementedException();
    }
    #endregion


    private void ChangeStageActionStatusToChallenge()
    {
        if (_stageActionStatus == Define.StageActionStatus.NotChallenge)
            StageActionStatus = Define.StageActionStatus.Challenge;
        else if (_stageActionStatus == Define.StageActionStatus.AutoChallenge)
            IsSpawnNamedMonster = true;
    }

    private void ClearSetting()
    {
        StageActionStatus = Define.StageActionStatus.NotChallenge;
        IsSpawnNamedMonster = false;
    }
}

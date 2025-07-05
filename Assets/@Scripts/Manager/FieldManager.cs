using System;
using System.Collections.Generic;
using UnityEngine;


public class RewardSystem
{
    Dictionary<Define.RewardType, int> _rewardList;

    public RewardSystem()
    {
        _rewardList = new();
    }

    public void SetReward(Define.RewardType rewardType, int rewardAmount)
    {
        _rewardList[rewardType] = rewardAmount;
    }

    public void GainReward(Vector3 pos)
    {
        foreach(var reward in _rewardList)
        {
            string rewardName = "";
            if (reward.Key == Define.RewardType.JourneyExp)
            {
                rewardName = "여정의 증표";
                PlayerManager.Instance.Player.GainJourneyExp(reward.Value);
            }
            else if(reward.Key == Define.RewardType.SilverCoin)
            {
                rewardName = "은화";
                PlayerManager.Instance.Player.Inventory.AddGoods(Define.GoodsType.SilverCoin, reward.Value);
            }
            else if(reward.Key == Define.RewardType.Gem)
            {
                rewardName = "젬";
                PlayerManager.Instance.Player.Inventory.AddGoods(Define.GoodsType.SilverCoin, reward.Value);
            }
            TextManager.Instance.ActivateRewardText(pos, rewardName, reward.Value);
        }
    }
}

public class FieldManager : Singleton<FieldManager>, IEventSubscriber, IDeactivateObject
{
    public event Action<Define.JourneyType, Define.ItemValue> OnJourneyEvent;
    public event Action OnFailedDungeonClear;

    StageController _stageController;
    SpawnController _spawnController;
    DungeonController _dungeonController;
    PlayerData _playerData;
    RewardSystem _rewardSystem;

    Define.JourneyType _currentType;

    int _stageCount;
    int _failedCount = 0;

    public StageController StageController
    {
        get { return _stageController; }
    }

    public DungeonController DungeonController
    {
        get { return _dungeonController; }
    }

    public int StageCount
    {
        get { return _stageController.StageCount; }
        set { _stageController.StageCount = value; }
    }

    public RewardSystem RewardSystem
    {
        get { return _rewardSystem; }
    }

    public int FailedCount
    {
        get { return _failedCount; }
    }

    public Define.JourneyType CurrentEventType
    {
        get { return _currentType; }
    }

    protected override void Initialize()
    {
        _dungeonController = new GameObject("DungeonController").AddComponent<DungeonController>();
        _stageController = new GameObject("StageController").AddComponent<StageController>();
        _spawnController = new GameObject("SpawnController").AddComponent<SpawnController>();

        _playerData = PlayerManager.Instance.Player.PlayerData;
        _rewardSystem = new RewardSystem();
        _stageCount = StageCount;
    }
    void Start()
    {
        PopupUIManager.Instance.ActivateJourneyInfo();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        PlayerManager.Instance.Player.OnPlayerCrossed += OnPlayerCross;
        PlayerManager.Instance.Player.OnPlayerDead += FailedDungeonClear;
        _dungeonController.OnDungeonClear += SuccessDungeonClear;
    }
    #endregion

    #region FieldUpdate
    //플레이어가 구역을 지나면 호출될 함수
    void OnPlayerCross()
    {
        int rnd;
        _stageCount = ++StageCount;
        //5의 배수마다 던전 두두둥장
        if (_stageCount % 5 == 0)
        {
            rnd = (int)Define.JourneyType.Dungeon;
            _rewardSystem.SetReward(Define.RewardType.JourneyExp, 2 * _stageCount);
        }
        else
        {
            rnd = UnityEngine.Random.Range(0, 100);
            rnd = 80;
            //80% 확률로 기타 오브젝트 등장
            if (rnd < 80)
            {
                rnd = (int)Define.JourneyType.OtherObject;
                _rewardSystem.SetReward(Define.RewardType.JourneyExp, 5 * _stageCount);
            }
            //10%의 확률로 보물 상자 등장
            else if (rnd < 90)
            {
                rnd = (int)Define.JourneyType.TreasureBox;
                Define.ItemValue treasureRank = SetTreasureRank();
                _rewardSystem.SetReward(Define.RewardType.JourneyExp, 10 * _stageCount * (int)treasureRank);
                _rewardSystem.SetReward(Define.RewardType.Gem, 10 * _stageCount * (int)treasureRank);
                _rewardSystem.SetReward(Define.RewardType.SilverCoin, 100 * _stageCount * (int)treasureRank);

                _currentType = (Define.JourneyType)rnd;
                OnJourneyEvent?.Invoke(_currentType, treasureRank);
                return;
            }
            //10%의 확률로 상인 등장
            else if (rnd < 100)
            {
                rnd = (int)Define.JourneyType.Merchant;
            }
        }
        _currentType = (Define.JourneyType)rnd;
        OnJourneyEvent?.Invoke(_currentType, Define.ItemValue.Common);
        PlayerManager.Instance.IsAutoMoving = false;
    }
    #endregion

    // * 보물상자의 등급을 정하는 함수
    Define.ItemValue SetTreasureRank()
    {
        int treasureRank = UnityEngine.Random.Range(1, 101);

        if (treasureRank < 65)
        {
            return Define.ItemValue.Uncommon;       //65% : Uncommon
        }
        else if (treasureRank < 85)
        {
            return Define.ItemValue.Rare;       //20% : Rare
        }
        else if (treasureRank < 95)
        {
            return Define.ItemValue.Epic;     //10% : Epic
        }
        else
            return Define.ItemValue.Legendary;  //5% : Legendary
    }

    // * 던전 클리어 시 실행
    void SuccessDungeonClear()
    {
        _rewardSystem.GainReward(PlayerManager.Instance.Player.transform.position + Vector3.up * 3);
        PlayerManager.Instance.Player.RemovePlayerBuff(_failedCount);
        _failedCount = 0;
        DungeonClear();
    }

    // * 던전을 깨지 못했을 떄 실행되는 함수 -> 플레이어가 죽거나 시간 초과 시 실행됨
    public void FailedDungeonClear()
    {
        StageCount -= 4;
        _stageCount = StageCount;
        OnFailedDungeonClear?.Invoke();
        if(_failedCount < 3)
        {
            _failedCount++;
            PlayerManager.Instance.Player.SetPlayerBuff();
        }
        DungeonClear();
    }

    void DungeonClear()
    {
        //플레이어 체력 초기화
        PlayerManager.Instance.Player.HP = PlayerManager.Instance.Player.PlayerData.HP;
        //타이머 초기화
        TimeManager.Instance.StopNamedMonsterTimer();
    }

    public void Deactivate()
    {

    }
}

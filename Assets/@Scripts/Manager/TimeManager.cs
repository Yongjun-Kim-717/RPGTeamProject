using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using extension;

public class TimeManager : Singleton<TimeManager>, IEventSubscriber
{
    public event Action<float> OnNamedMonsterTimeChanged;
    public event Action<Define.TimeOfDayType> OnColorChanged;
    public event Action<Define.TimeOfDayType> OnTimeSpeedChanged;

    WaitForSeconds _standardTime = new WaitForSeconds(1f);
    private Coroutine _namedMonsterCoroutine;

    float _monsterTime;
    public float _dayTime;
    Define.TimeOfDayType _currentTime;
    bool _isDoubleSpeed;
    bool _isSkyBoxChange = false;
    float _toKey = 1;

    public bool IsDoubleSpeed
    {
        get { return _isDoubleSpeed; }
        set
        {
            _isDoubleSpeed = value;
            _toKey = _isDoubleSpeed ? 0.1f : 1;
            _standardTime = new WaitForSeconds(_toKey);
            EnvironmentManager.Instance.ToKey = _toKey;
            OnTimeSpeedChanged?.Invoke(_currentTime);
        }
    }

    public bool IsSkyBoxChange
    {
        get { return _isSkyBoxChange; }
        set
        {
            _isSkyBoxChange = value;
            if (!_isSkyBoxChange)
                StartDay();
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    private void OnEnable()
    {
        StartDay();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += StartNamedMonsterStage;
    }
    #endregion

    #region NamedMonsterStageTimer
    public void StartNamedMonsterStage()
    {
        _monsterTime = 100;
        _namedMonsterCoroutine = StartCoroutine(NamedMonsterTimer());
    }

    IEnumerator NamedMonsterTimer()
    {
        while (_monsterTime > 0)
        {
            yield return _standardTime;
            _monsterTime -= 1;
            if (_monsterTime <= 0)
            {
                //던전 클리어 실패 함수 호출
                FieldManager.Instance.FailedDungeonClear();
                break;
            }
            OnNamedMonsterTimeChanged?.Invoke(_monsterTime);
        }
    }

    public void StopNamedMonsterTimer()
    {
        if (_namedMonsterCoroutine != null)
        {
            StopCoroutine(_namedMonsterCoroutine);
        }
        _namedMonsterCoroutine = null;
    }
    #endregion

    #region DayTimer
    public void StartDay()
    {
        StartCoroutine(DayTimer());
    }

    IEnumerator DayTimer()
    {
        while (true)
        {
            yield return _standardTime;
            _dayTime += 1;

            if (Define.ColorChangeTimeList.TryGetValue(_dayTime, out var newColorTimeOfDay))
            {
                //현재 스카이박스 색 변경
                OnColorChanged?.Invoke(newColorTimeOfDay);
                _currentTime = Extension.GetNextType(newColorTimeOfDay);
            }

            if (_dayTime >= 360f)
            {
                _dayTime = 0f;
            }
        }
    }

    #endregion
}

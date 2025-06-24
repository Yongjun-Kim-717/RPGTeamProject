using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>, IEventSubscriber
{
    public event Action<float> OnNamedMonsterTimeChanged;
    public event Action OnGainedRecordTimeChanged;
    public event Action<Define.TimeOfDayType> OnColorChanged;
    public event Action<Define.TimeOfDayType> OnSkyBoxChanged;
    public event Action<Define.TimeOfDayType> OnTimeSpeedChanged;

    WaitForSeconds _standardTime = new WaitForSeconds(1f);

    private Dictionary<float, Define.TimeOfDayType> _colorChangeTransitions;
    private Dictionary<float, Define.TimeOfDayType> _skyBoxChangeTransitions;

    float _monsterTime;
    public float _dayTime;
    float _duration = 12;
    Define.TimeOfDayType _currentTime;
    bool _isDoubleSpeed;
    bool _isPlaying = true;
    bool _isSkyBoxChange = false;
    float _toKey = 1;

    public float Duration
    {
        get 
        { 
            return _duration * _toKey; 
        }
    }

    public bool IsDoubleSpeed
    {
        get { return _isDoubleSpeed; }
        set
        {
            _isDoubleSpeed = value;
            _toKey = _isDoubleSpeed ? 0.2f : 1;
            _standardTime = new WaitForSeconds(_toKey); // 레퍼런스타입이라 new로 매번 생성하면 GC 메모리 낭비
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
            if(!_isSkyBoxChange)
                StartDay();
        }
    }

    protected override void Initialize()
    {
        base.Initialize();
        _colorChangeTransitions = new()
        {
            { 12f, Define.TimeOfDayType.Noon }, //7초에 낮 색 변경
            { 22f, Define.TimeOfDayType.Evening },
            { 37f, Define.TimeOfDayType.Night },
            { 51f, Define.TimeOfDayType.Morning }
        };
        //값에 해당하는 스카이박스 변경될 때의 시각
        _skyBoxChangeTransitions = new()
        {
            { GetColorChangeTime(Define.TimeOfDayType.Noon)+4f, Define.TimeOfDayType.Noon },
            { GetColorChangeTime(Define.TimeOfDayType.Evening)+4f, Define.TimeOfDayType.Evening },
            { GetColorChangeTime(Define.TimeOfDayType.Night)+5f, Define.TimeOfDayType.Night },
            { GetColorChangeTime(Define.TimeOfDayType.Morning)+1f, Define.TimeOfDayType.Morning },
        };
    }

    private void OnEnable()
    {
        StartGainedRecord();
        StartDay();
    }

    #region IEventSubscriber
    public void Subscribe()
    {
        DungeonManager.Instance.OnSpawnNamedMonster += StartNamedMonsterStage;
    }
    #endregion

    #region NamedMonsterStageTimer
    public void StartNamedMonsterStage()
    {
        _monsterTime = 100;
        StartCoroutine(NamedMonsterTimer());
    }

    IEnumerator NamedMonsterTimer()
    {
        while (_monsterTime > 0)
        {
            yield return _standardTime;
            _monsterTime -= 1;
            if (_monsterTime <= 0)
                break;
            OnNamedMonsterTimeChanged?.Invoke(_monsterTime);
        }
    }
    #endregion

    #region GainRecord Timer
    public void StartGainedRecord()
    {
        StartCoroutine(GainedRecordTimer());
    }
    IEnumerator GainedRecordTimer()
    {
        while (_isPlaying)
        {
            yield return _standardTime;
            OnGainedRecordTimeChanged?.Invoke();
        }
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
            Debug.Log(_dayTime);
            
            if (_colorChangeTransitions.TryGetValue(_dayTime, out var newColorTimeOfDay))
            {
                //현재 스카이박스 색 변경
                OnColorChanged?.Invoke(newColorTimeOfDay);
                _currentTime = GetNextType(newColorTimeOfDay);
            }
            //newSkyBox를 다음 스카이박스로 변경
            else if(_skyBoxChangeTransitions.TryGetValue(_dayTime, out var curSkyBox))
            {
                //그다음 스카이박스가 변경될 시각 - 현재 스카이박스가 끝날 시각
                _duration = GetTime(GetNextType(curSkyBox)) - GetTime(curSkyBox);
                if(curSkyBox == Define.TimeOfDayType.Morning)
                {
                    _duration = GetTime(GetNextType(curSkyBox));
                }
                OnSkyBoxChanged?.Invoke(GetNextType(curSkyBox));
            }
            if (_dayTime >= 53f)
            {
                _dayTime = 0f;
            }
        }
    }

    float GetColorChangeTime(Define.TimeOfDayType type)
    {
        return _colorChangeTransitions.FirstOrDefault(x => x.Value == type).Key;
    }

    float GetTime(Define.TimeOfDayType type)
    {
        return _skyBoxChangeTransitions.FirstOrDefault(x => x.Value == type).Key;
    }
    Define.TimeOfDayType GetNextType(Define.TimeOfDayType type)
    {
        return type switch
        {
            Define.TimeOfDayType.Noon => Define.TimeOfDayType.Evening,
            Define.TimeOfDayType.Evening => Define.TimeOfDayType.Night,
            Define.TimeOfDayType.Night => Define.TimeOfDayType.Morning,
            Define.TimeOfDayType.Morning => Define.TimeOfDayType.Noon,
            _ => 0
        };
    }

    #endregion
}

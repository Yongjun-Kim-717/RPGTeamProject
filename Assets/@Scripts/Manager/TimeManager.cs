
using System;
using System.Collections;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    public Action<float> OnTimeChanged;
    [SerializeField] float _time;
    [SerializeField] float _monsterTime;


    private void OnEnable()
    {
        _monsterTime = 180;
        StartCoroutine(StartTimer());
    }

    IEnumerator StartTimer()
    {
        while (_monsterTime > 0)
        {
            yield return new WaitForSeconds(1f);
            _monsterTime -= 1;
            OnTimeChanged?.Invoke(_monsterTime);
        }
    }
}

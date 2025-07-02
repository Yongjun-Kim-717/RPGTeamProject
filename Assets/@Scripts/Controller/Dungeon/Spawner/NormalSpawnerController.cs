using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * NormalSpawner 컨트롤러 스크립트
public class NormalSpawnerController : MonoBehaviour
{
    private string _monsterName;            // 스폰 될 몬스터 이름
    private WaitForSeconds _spawnInterval;  // 스폰 간격
    private float _monsterInterval = 1f;
    private Vector3 _spawnPos;
    private Vector3 _offset;

    private int _limitOfNormalMonsterCount = 80;

    private Coroutine _spawnCoroutine;

    // 스포너 위치 설정
    public void SetSpawnerPos(Vector3 offSet)
    {
        _offset = offSet;
        _spawnPos = _offset;
    }

    // 스포너 설정 및 스폰 시작
    public void SetSpawnerOn(string monsterName, float spawnInterval, float monsterInterval)
    {
        _monsterName = monsterName;
        _spawnInterval = new WaitForSeconds(spawnInterval);
        _monsterInterval = monsterInterval;
        _spawnCoroutine = StartCoroutine(StartSpawn());
    }

    // 스포너 해제
    public void SetSpawnerOff()
    {
        StopCoroutine(_spawnCoroutine);
    }

    //* 스폰 시작 코루틴
    //- 게이트 생성 -> 게이트 최대 크기 달성 시 몬스터 스폰
    IEnumerator StartSpawn()
    {
        while(true)
        {
            SpawnGate();
            yield return new WaitForSeconds(0.5f);
            SpawnGate();
            yield return _spawnInterval;
        }
    }

    // * 게이트 생성 메서드
    //- 활성화된 몬스터 수의 최대 마릿 수 검사 및 생성 결정
    void SpawnGate()
    {
        if(NormalMonsterController.s_AliveCount <= _limitOfNormalMonsterCount)
        {
            _spawnPos = GetRandomPos();

            GameObject monsterGate = PoolManager.Instance.GetObjectFromPool<MonsterGateController>(_spawnPos, "HellGate");
            MonsterGateController monsterGateController = monsterGate.GetComponent<MonsterGateController>();
            monsterGateController.SetRotation(_offset);
            monsterGateController.OnGateOpen -= SpawnMonsters;
            monsterGateController.OnGateOpen += SpawnMonsters;
        }
    }

    // * 몬스터 생성 메서드
    void SpawnMonsters(Vector3 spawnPos)
    {
        spawnPos -= Vector3.up * 3;
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(spawnPos + _monsterInterval * Vector3.right, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(spawnPos + _monsterInterval * Vector3.left, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(spawnPos + _monsterInterval * Vector3.forward, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(spawnPos + _monsterInterval * Vector3.back, _monsterName);
    }

    // * 랜덤 스폰 위치 제공 메서드
    Vector3 GetRandomPos()
    {
        float randomDistX = Random.Range(-20, 20);
        float randomDistY = Random.Range(5, 10);
        float randomDistZ = Random.Range(-20, 20);

        Vector3 randomPos = new Vector3(randomDistX, randomDistY, randomDistZ);
        return _offset + randomPos;
    }
}

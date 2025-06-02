using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// * NormalSpawner 컨트롤러 스크립트
public class NormalSpawnerController : MonoBehaviour
{
    private List<Vector3> _spawnPosList;

    private string _monsterName;            // 스폰 될 몬스터 이름
    private WaitForSeconds _spawnInterval;  // 스폰 간격

    private void OnEnable()
    {
        Initialize();
    }

    private void OnDisable()
    {
        SetSpawnerOff();
    }

    void Initialize()
    {
        _spawnPosList = new List<Vector3>();
    }

    // 스포너 위치 설정
    public void SetSpawnerPos(Vector3 spawnerPos)
    {

    }

    // 스포너 설정 및 스폰 시작
    public void SetSpawnerOn(string monsterName, WaitForSeconds spawnInterval)
    {
        _monsterName = monsterName;
        _spawnInterval = spawnInterval;
        StartCoroutine(StartSpawn());
    }

    // 스포너 해제
    void SetSpawnerOff()
    {
        StopCoroutine(StartSpawn());
    }

    IEnumerator StartSpawn()
    {
        SpawnMonsters();
        yield return _spawnInterval;
    }

    void SpawnMonsters()
    {
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(transform.position, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(transform.position + Vector3.right, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(transform.position + Vector3.left, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(transform.position + Vector3.forward, _monsterName);
        PoolManager.Instance.GetObjectFromPool<NormalMonsterController>(transform.position + Vector3.back, _monsterName);
    }
}

using UnityEngine;

public class NamedSpawnerController : MonoBehaviour
{
    private Vector3 _spawnPos;

    private string _monsterName;            // 스폰 될 몬스터 이름

    public void SetSpawnerPos(Vector3 offset)
    {
        _spawnPos = Define.NamedMonsterSpawnSpot + offset;
    }

    public void SetSpawnerOn(string monsterName)
    {
        _monsterName = monsterName;
        SpawnNamedMonster();
    }

    private void SpawnNamedMonster()
    {
        PoolManager.Instance.GetObjectFromPool<NamedMonsterController>(_spawnPos, _monsterName);
    }

    public void SetSpawnerOff()
    {

    }
}

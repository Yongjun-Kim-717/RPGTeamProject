using UnityEngine;
using extension;
using System.Collections.Generic;
using Unity.VisualScripting;

//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnController : MonoBehaviour
{
    Dictionary<string, GameObject> _fieldObjectSpawnSpotList;
    Dictionary<string, GameObject> _fieldObjectList;
    private NormalSpawnerController _normalSpawner;
    private NamedSpawnerController _namedSpawner;

    private GameObject _monsterParentPool;

    [SerializeField] float _spawnInterval = 3f;
    [SerializeField] float _monsterInterval = 2f;

    public NormalSpawnerController NormalSpawner
    {
        get { return _normalSpawner; }
    }    

    void Awake()
    {
        Initialize();
    }

    void Initialize()
    {
        _monsterParentPool = new GameObject("MonsterPool");

        _fieldObjectSpawnSpotList = new Dictionary<string, GameObject>();
        _fieldObjectList = new Dictionary<string, GameObject>();

        _fieldObjectSpawnSpotList = ObjectManager.Instance.FieldObjectSpawnSpotList;
        foreach (var obj in ObjectManager.Instance.FieldObjectList)
        {
            GameObject newObj = Instantiate(obj.Value);
            _fieldObjectList.Add(obj.Key, newObj);
        }

        DeactivateObject();
        GenerateNormalSpawner();
        GenerateNamedSpawner();
    }

    void DeactivateObject()
    {
        foreach (var obj in _fieldObjectList)
        {
            obj.Value.SetActive(false);
        }
    }
    private void OnEnable()
    {
        Subscribe();
    }

    void Subscribe()
    {
        FieldManager.Instance.DungeonController.OnDungeonEnter += SetNormalSpawnerOn;
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += SetNormalSpawnerOff;
        FieldManager.Instance.DungeonController.OnSpawnNamedMonster += SetNamedSpawnerOn;
        FieldManager.Instance.DungeonController.OnDungeonExit += SetNamedSpawnerOff;
        FieldManager.Instance.DungeonController.OnDungeonExit += SetNormalSpawnerOff;
        FieldManager.Instance.OnJourneyEvent += SetEvent;
    }

    #region GenerateSpawner
    void GenerateNormalSpawner()
    {
        _normalSpawner = new GameObject("NormalMonsterSpawner").GetOrAddComponent<NormalSpawnerController>();
    }
    void GenerateNamedSpawner()
    {
        _namedSpawner = new GameObject("NamedMonsterSpawner").GetOrAddComponent<NamedSpawnerController>();
    }
    #endregion

    #region Spawner
    void SetNormalSpawnerOn()
    {
        _normalSpawner.gameObject.SetActive(true);
        _normalSpawner.SetSpawnerPos(FieldManager.Instance.DungeonController.DungeonOffSet);
        _normalSpawner.SetSpawnerOn("Demon", _spawnInterval, _monsterInterval);
    }

    void SetNormalSpawnerOff()
    {
        _normalSpawner.SetSpawnerOff();
        _normalSpawner.gameObject.SetActive(false);
    }

    void SetNamedSpawnerOn()
    {
        _namedSpawner.gameObject.SetActive(true);
        _namedSpawner.SetSpawnerPos(FieldManager.Instance.DungeonController.DungeonOffSet, _monsterParentPool);
        _namedSpawner.SetSpawnerOn("Goblin");
    }

    void SetNamedSpawnerOff()
    {
        _namedSpawner.gameObject.SetActive(false);
        _namedSpawner.SetSpawnerOff();
    }
    #endregion

    //FieldManager에서 이벤트 발생 -> PlayerManager로 받아와 FieldManager를 거치지 않고 할까??????
    //현재대로만 한다면 PlayerManager의 Player 프로퍼티를 받아 바로 이벤트 구독하자
    void SetEvent(Define.JourneyType type, Define.ItemValue treasureRank)
    {
        DeactivateObject();

        //필드 지날 때 발생하는 이벤트가 던전이 아닐 때만
        if (type != Define.JourneyType.Dungeon)
            ActivateObject(type, treasureRank);
    }

    void ActivateObject(Define.JourneyType type, Define.ItemValue treasureRank)
    {
        string spawnNumber = Random.Range(1, 6).ToString();

        //랜덤 위치 받기
        Vector3 spawnPos = _fieldObjectSpawnSpotList["FieldObjectSpawnSpot" + spawnNumber].transform.position;

        string objectName = "TreasureBox";
        switch (type)
        {
            case Define.JourneyType.Merchant:
                objectName = "Merchant"; break;
            case Define.JourneyType.OtherObject:
                //우선은 그냥 확률 똑같이
                objectName = "OtherObject" + Random.Range(1, 4).ToString(); break;
            case Define.JourneyType.TreasureBox:
                _fieldObjectList[objectName].GetComponent<TreasureBoxController>().TreasureRank = treasureRank;
                break;
        }
        _fieldObjectList[objectName].transform.position = spawnPos;
        _fieldObjectList[objectName].SetActive(true);
    }
}

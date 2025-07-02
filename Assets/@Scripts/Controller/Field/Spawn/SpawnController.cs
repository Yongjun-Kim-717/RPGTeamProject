using UnityEngine;
using extension;
using System.Collections.Generic;
using Unity.VisualScripting;

//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnController : MonoBehaviour
{
    Dictionary<string, GameObject> _fieldObjectSpawnSpotList;
    private Dictionary<string, GameObject> _fieldObjectList;

    private NormalSpawnerController _normalSpawner;
    private NamedSpawnerController _namedSpawner;


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
        foreach(var obj in _fieldObjectList)
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


    #region MonsterSpawn Function
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
        _namedSpawner.SetSpawnerPos(FieldManager.Instance.DungeonController.DungeonOffSet);
        _namedSpawner.SetSpawnerOn("Goblin");
    }

    void SetNamedSpawnerOff()
    {
        _namedSpawner.SetSpawnerOff();
        _namedSpawner.gameObject.SetActive(false);
    }
    #endregion

    //FieldManager에서 이벤트 발생 -> PlayerManager로 받아와 FieldManager를 거치지 않고 할까??????
    //현재대로만 한다면 PlayerManager의 Player 프로퍼티를 받아 바로 이벤트 구독하자
    void SetEvent(Define.JourneyEventType type)
    {
        //기존에 활성화 됐던 오브젝트들 비활성화
        DeactivateObject();

        //오브젝트 활성화
        ActivateObject(type);
    }

    void ActivateObject(Define.JourneyEventType type)
    {
        string spawnNumber = Random.Range(1, 5).ToString(); 

        //랜덤 위치 받기
        Vector3 spawnPos = _fieldObjectSpawnSpotList["FieldObjectSpawnSpot" + spawnNumber].transform.position;
        
        string objectName;
        switch (type)
        {
            case Define.JourneyEventType.TreasureBox:
                objectName = "TreasureBox"; break;
            case Define.JourneyEventType.Merchant:
                objectName = "Merchant"; break;
            default:
                objectName = "Other"; break;
        }
        _fieldObjectList[objectName].transform.position = spawnPos;
        _fieldObjectList[objectName].SetActive(true);
    }
}

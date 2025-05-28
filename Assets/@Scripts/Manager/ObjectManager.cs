using System;
using System.Collections.Generic;
using UnityEngine;
using static extension.Extension;

// * ObjectManager 스크립트
//- 모든 프리팹을 Load한다.
public class ObjectManager : Singleton<ObjectManager>
{

    #region Resources
    // * Dictionary<이름, 프리팹 오브젝트>
    private Dictionary<string, GameObject> _monsterResourceList;
    private Dictionary<string, GameObject> _namedMonsterResourceList;
    private Dictionary<string, GameObject> _monsterSkillResourceList;
    private Dictionary<string, GameObject> _monsterSkillHitResourceList;
    private Dictionary<string, GameObject> _playerSkillResourceList;
    private Dictionary<string, GameObject> _playerSkillHitResourceList;

    // * GameObject : 인게임 오브젝트
    private GameObject _playerResource;
    private GameObject _dungeonWallResource;
    private GameObject _dungeonPortalResource;
    private GameObject _shieldEffectResource;

    // * UI Object
    private GameObject _popupCanvas;
    private GameObject _popupPanel;

    // * 프로퍼티
    public Dictionary<string, GameObject> MonsterResourceList 
    {
        get
        {
            if (NullCheck(_monsterResourceList))
                return null;
            return _monsterResourceList;
        }
    }
    public Dictionary<string, GameObject> NamedMonsterResourceList
    { 
        get 
        {
            if (NullCheck(_namedMonsterResourceList))
                return null;
            return _namedMonsterResourceList;
        } 
    }
    public Dictionary<string, GameObject> MonsterSkillResourceList
    { 
        get
        {
            if (NullCheck(_monsterSkillResourceList))
                return null;
            return _monsterSkillResourceList;
        } 
    }
    public Dictionary <string, GameObject> MonsterSkillHitResourceList
    {
        get
        {
            if (NullCheck(_monsterSkillHitResourceList))
                return null;
            return _monsterSkillHitResourceList;
        }
    }
    public Dictionary<string, GameObject> PlayerSkillResourceList 
    {
        get
        {
            if (NullCheck(_playerSkillResourceList))
                return null;
            return _playerSkillResourceList; 
        }
    }
    public Dictionary <string, GameObject> PlayerSkillHitResourceList
    {
        get
        {
            if (NullCheck(_playerSkillHitResourceList))
                return null;
            return _playerSkillHitResourceList;
        }
    }

    public GameObject PlayerResource 
    { 
        get 
        {
            if (NullCheck(_playerResource))
                return null;
            return _playerResource;
        } 
    }
    public GameObject DungeonWallResource
    {
        get
        {
            if(NullCheck(_dungeonWallResource))
                return null;
            return _dungeonWallResource;
        }
    }
    public GameObject DungeonPortalResource
    {
        get
        {
            if (NullCheck(_dungeonPortalResource))
                return null;
            return _dungeonPortalResource;
        }
    }
    public GameObject ShieldEffectResource
    {
        get
        {
            if (NullCheck(_shieldEffectResource))
                return null;
            return _shieldEffectResource;
        }
    }

    public GameObject PopupCanvas
    {
        get
        {
            if (NullCheck(_popupCanvas))
                return null;
            return _popupCanvas;
        }
    }
    public GameObject PopupPanel
    {
        get
        {
            if (NullCheck(_popupPanel))
                return null;
            return _popupPanel;
        }
    }
    #endregion

    #region Override
    // * 초기화 메서드 (재정의)
    protected override void Initialize()
    {
        base.Initialize();
        _monsterResourceList = new Dictionary<string, GameObject>();
        _namedMonsterResourceList = new Dictionary<string, GameObject>();
        _monsterSkillResourceList = new Dictionary<string, GameObject>();
        _monsterSkillHitResourceList = new Dictionary<string, GameObject>();
        _playerSkillResourceList = new Dictionary<string, GameObject>();
        _playerSkillHitResourceList = new Dictionary<string, GameObject>();
    }
    #endregion

    #region ResourceLoadMethod
    // * 리소스 로드 메서드
    public void ResourceLoad()
    {
        PlayerResourceLoad();
        DungeonObjectResourceLoad();
        SkillResourceLoad();
        MonsterResourceLoad();
        PopupUIResourceLoad();
    }

    // * 플레이어 리소스 로드 메서드
    private void PlayerResourceLoad()
    {
        _playerResource = Resources.Load<GameObject>(Define.PlayerPath);
    }
    // * 던전 결계 리소스 로드 메서드
    private void DungeonObjectResourceLoad()
    {
        _shieldEffectResource = Resources.Load<GameObject>(Define.ShieldEffectPath);
        _dungeonWallResource = Resources.Load<GameObject>(Define.DungeonWallPath);
        _dungeonPortalResource = Resources.Load<GameObject>(Define.DungeonPortalPath);
    }
    // * 스킬 리소스 로드 메서드
    private void SkillResourceLoad()
    {
        if(!NullCheck(_monsterSkillResourceList, _monsterSkillHitResourceList, _playerSkillResourceList, _playerSkillResourceList))
        {
            Resources.LoadAll<GameObject>(Define.MonsterSkillPath).ToList(_monsterSkillResourceList);
            Resources.LoadAll<GameObject>(Define.MonsterSkillHitEffectPath).ToList(_monsterSkillHitResourceList);
            Resources.LoadAll<GameObject>(Define.PlayerSkillPath).ToList(_playerSkillResourceList);
            Resources.LoadAll<GameObject>(Define.PlayerSkillHitEffectPath).ToList(_playerSkillResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of skill list is null");
        }
    }
    // * 몬스터 리소스 로드 메서드
    private void MonsterResourceLoad()
    {
        if (!NullCheck(_monsterResourceList, _namedMonsterResourceList))
        {
            Resources.LoadAll<GameObject>(Define.MonsterPath).ToList(_monsterResourceList);
            Resources.LoadAll<GameObject>(Define.NamedMonsterPath).ToList(_namedMonsterResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of monster list is null");
        }
    }
    // * 팝업 UI 리소스 로드 메서드
    private void PopupUIResourceLoad()
    {
        _popupCanvas = Resources.Load<GameObject>(Define.PopupUICanvasPath);
        _popupPanel = Resources.Load<GameObject>(Define.PopupEnterDungeonPanelPath);
    }
    #endregion

    #region InstantiateObjectMethod
    public GameObject GetObject<T>(Vector3 spawnPos, string name) where T : MonoBehaviour
    {
        Type type = typeof(T);
        if(type == typeof(PlayerController))
        {
            GameObject obj = Instantiate(PlayerResource, spawnPos, Quaternion.identity);
            PlayerController playerController = obj.GetOrAddComponent<PlayerController>();
            return obj;
        }
        else if(type == typeof(NormalMonsterController))
        {
            GameObject obj = Instantiate(MonsterResourceList[name], spawnPos, Quaternion.identity);
            NormalMonsterController normalMonsterController = obj.GetOrAddComponent<NormalMonsterController>();
            return obj;
        }
        else if(type == typeof(Skill))
        {
            GameObject obj = Instantiate<GameObject>(PlayerSkillResourceList[name], spawnPos, Quaternion.identity);
            return obj;
        }
        return null;
    }
    #endregion
}

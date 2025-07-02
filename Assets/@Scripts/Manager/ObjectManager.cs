using extension;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static extension.Extension;

// * ObjectManager 스크립트
//- 모든 프리팹을 Load한다.
public class ObjectManager : Singleton<ObjectManager>
{
    #region Resources
    // * Dictionary<이름, 프리팹 오브젝트>
    private Dictionary<string, GameObject> _normalMonsterResourceList;
    private Dictionary<string, GameObject> _namedMonsterResourceList;
    private Dictionary<string, GameObject> _monsterSkillResourceList;
    private Dictionary<string, GameObject> _monsterSkillHitEffectResourceList;
    private Dictionary<string, GameObject> _playerSkillResourceList;
    private Dictionary<string, GameObject> _playerSkillHitEffectResourceList;
    private Dictionary<string, GameObject> _damageTextResourceList;
    private Dictionary<string, Material> _skyBoxResourceList;
    private Dictionary<string, JourneyRankData> _journeyRankResourceList;
    private Dictionary<string, GameObject> _fieldObjectSpawnSpotList;
    private Dictionary<string, GameObject> _itemSlotList;
    private Dictionary<string, GameObject> _fieldObjectList;

    // * GameObject : 인게임 오브젝트
    private GameObject _playerResource;
    private GameObject _dungeonWallResource;
    private GameObject _dungeonPortalResource;
    private GameObject _shieldEffectResource;
    private GameObject _monsterGateResource;
    private GameObject _backgroundResource;

    // * UI Object
    private GameObject _uiGame;
    private GameObject _popupCanvas;
    private GameObject _popupPanel;
    private GameObject _popupGainedRecordInfo;
    private GameObject _popupJourneyInfo;
    private GameObject _popupStageInfo;
    private GameObject _popupNamedMonsterInfo;
    private GameObject _popupStatusPanel;
    private GameObject _popupInventoryPanel;
    private GameObject _popupSkillInventory;
    private GameObject _popupMerchantPanel;

    private GameObject _systemTextResource;
    private GameObject _treasureTextResource;

    private GameObject _playerVitalCanvas;
    private GameObject _playerVitalResource;

    // * TreasureBox Effect 어디에 놓을가요
    private GameObject _treasureBoxOpenEffectResource;

    // * CutScene
    private GameObject _goblinKingCutScene;
    private GameObject _followCam;
    private GameObject _cutSceneCam;
    

    // * 프로퍼티
    public Dictionary<string, GameObject> NormalMonsterResourceList 
    {
        get
        {
            if (NullCheck(_normalMonsterResourceList))
                return null;
            return _normalMonsterResourceList;
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
    public Dictionary <string, GameObject> MonsterSkillHitEffectResourceList
    {
        get
        {
            if (NullCheck(_monsterSkillHitEffectResourceList))
                return null;
            return _monsterSkillHitEffectResourceList;
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
    public Dictionary<string, GameObject> PlayerSkillHitEffectResourceList
    {
        get
        {
            if (NullCheck(_playerSkillHitEffectResourceList))
                return null;
            return _playerSkillHitEffectResourceList;
        }
    }
    public Dictionary<string, GameObject > DamageTextResourceList
    {
        get
        {
            if (NullCheck(_damageTextResourceList))
                return null;
            return _damageTextResourceList;
        }
    }
    public Dictionary<string, Material> SkyBoxResourceList
    {
        get
        {
            if (NullCheck(_skyBoxResourceList))
                return null;
            return _skyBoxResourceList;
        }
    }
    public Dictionary<string, JourneyRankData> JourneyRankResourceList
    {
        get 
        {
            if (NullCheck(_journeyRankResourceList))
                return null;
            return _journeyRankResourceList;
        }
    }
    public Dictionary<string, GameObject> FieldObjectSpawnSpotList
    {
        get
        {
            if (NullCheck(_fieldObjectSpawnSpotList))
                return null;
            return _fieldObjectSpawnSpotList;
        }
    }
    public Dictionary<string, GameObject> ItemSlotList
    {
        get
        {
            if (NullCheck(_itemSlotList))
                return null;
            return _itemSlotList;
        }
    }


    public Dictionary<string, GameObject> FieldObjectList
    {
        get
        {
            if (NullCheck(_fieldObjectList))
                return null;
            return _fieldObjectList;
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

    public GameObject MonsterGateResource
    {
        get
        {
            if (NullCheck(_monsterGateResource))
                return null;
            return _monsterGateResource;
        }
    }

    public GameObject BackgroundResource
    {
        get
        {
            if (NullCheck(_backgroundResource))
                return null;
            return _backgroundResource;
        }
    }

    public GameObject UIGame
    {
        get
        {
            if (NullCheck(_uiGame))
                return null;
            return _uiGame;
        }
    }

    public GameObject SystemTextResource
    {
        get
        {
            if (NullCheck(_systemTextResource))
                return null;
            return _systemTextResource;
        }
    }

    public GameObject TreasureTextResource
    {
        get
        {
            if (NullCheck(_treasureTextResource))
                return null;
            return _treasureTextResource;
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

    public GameObject PopupJourneyInfo
    {
        get
        {
            if (NullCheck(_popupJourneyInfo))
                return null;
            return _popupJourneyInfo;
        }
    }
    public GameObject PopupStageInfo
    {
        get
        {
            if (NullCheck(_popupStageInfo))
                return null;
            return _popupStageInfo;
        }
    }

    public GameObject PopupNamedMonsterInfo
    {
        get
        {
            if (NullCheck(_popupNamedMonsterInfo))
                return null;
            return _popupNamedMonsterInfo;
        }
    }

    public GameObject PopupStatusPanel
    {
        get
        {
            if (NullCheck(_popupStatusPanel))
                return null;
            return _popupStatusPanel;
        }
    }
    public GameObject PopupInventoryPanel
    {
        get
        {
            if (NullCheck(_popupInventoryPanel))
                return null;
            return _popupInventoryPanel;
        }
    }
    public GameObject PopupSkillInventory
    {
        get
        {
            if (NullCheck(_popupSkillInventory))
                return null;
            return _popupSkillInventory;
        }
    }
    public GameObject PopupGainedRecordPanel
    {
        get
        {
            if (NullCheck(_popupGainedRecordInfo))
            {
                return null;
            }
            return _popupGainedRecordInfo;
        }
    }

    public GameObject PopupMerchantPanel
    {
        get
        {
            if (NullCheck(_popupMerchantPanel))
                return null;
            return _popupMerchantPanel;
        }
    }

    public GameObject PlayerVitalCanvas
    {
        get
        {
            if (NullCheck(_playerVitalCanvas))
                return null;
            return _playerVitalCanvas;
        }
    }

    public GameObject PlayerVitalResource
    {
        get
        {
            if (NullCheck(_playerVitalResource))
                return null;
            return _playerVitalResource;
        }
    }

    public GameObject TreasureBoxOpenEffectResource
    {
        get
        {
            if (NullCheck(_treasureBoxOpenEffectResource))
                return null;
            return _treasureBoxOpenEffectResource;
        }
    }

    public GameObject GoblinKingCutScene
    {
        get
        {
            if (NullCheck(_goblinKingCutScene))
                return null;
            return _goblinKingCutScene;
        }
    }

    public GameObject FollowCam
    {
        get
        {
            if (NullCheck(_followCam))
                return null;
            return _followCam;
        }
    }

    public GameObject CutSceneCam
    {
        get
        {
            if (NullCheck(_cutSceneCam))
                return null;
            return _cutSceneCam;
        }
    }
    #endregion

    #region Override
    // * 초기화 메서드 (재정의)
    protected override void Initialize()
    {
        base.Initialize();
        _normalMonsterResourceList = new Dictionary<string, GameObject>();
        _namedMonsterResourceList = new Dictionary<string, GameObject>();
        _monsterSkillResourceList = new Dictionary<string, GameObject>();
        _monsterSkillHitEffectResourceList = new Dictionary<string, GameObject>();
        _playerSkillResourceList = new Dictionary<string, GameObject>();
        _playerSkillHitEffectResourceList = new Dictionary<string, GameObject>();
        _damageTextResourceList = new Dictionary<string, GameObject>();
        _skyBoxResourceList = new Dictionary<string, Material>();
        _journeyRankResourceList = new Dictionary<string, JourneyRankData>();
        _fieldObjectSpawnSpotList = new Dictionary<string, GameObject>();
        _itemSlotList = new Dictionary<string, GameObject>();
        _fieldObjectList = new Dictionary<string, GameObject>();
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
        FieldObjectResourceLoad();
        FieldObjectSpawnSpotLoad();
        TreasureBoxOpenEffectLoad();
        DamageTextResourceLoad();
        SkyBoxResourceLoad();
        JourneyRankResourceLoad();
        EnvironmentResourceLoad();
        UIResourceLoad();
        PopupUIResourceLoad();
        PlayerVitalResourceLoad();
        CutSceneResourceLoad();
        CameraResourceLoad();
        ItemSlotResourceLoad();
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
        _monsterGateResource = Resources.Load<GameObject>(Define.MonsterGatePath);
    }
    // * 스킬 리소스 로드 메서드
    private void SkillResourceLoad()
    {
        if(!NullCheck(_monsterSkillResourceList, _monsterSkillHitEffectResourceList, _playerSkillResourceList, _playerSkillResourceList))
        {
            Resources.LoadAll<GameObject>(Define.MonsterSkillPath).ToList(_monsterSkillResourceList);
            Resources.LoadAll<GameObject>(Define.MonsterSkillHitEffectPath).ToList(_monsterSkillHitEffectResourceList);
            Resources.LoadAll<GameObject>(Define.PlayerSkillPath).ToList(_playerSkillResourceList);
            Resources.LoadAll<GameObject>(Define.PlayerSkillHitEffectPath).ToList(_playerSkillHitEffectResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of skill list is null");
        }
    }
    // * 몬스터 리소스 로드 메서드
    private void MonsterResourceLoad()
    {
        if (!NullCheck(_normalMonsterResourceList, _namedMonsterResourceList))
        {
            Resources.LoadAll<GameObject>(Define.MonsterPath).ToList(_normalMonsterResourceList);
            Resources.LoadAll<GameObject>(Define.NamedMonsterPath).ToList(_namedMonsterResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of monster list is null");
        }
    }

    // * 필드 리소스 로드 메서드
    private void FieldObjectResourceLoad()
    {
        if (!NullCheck(_fieldObjectList))
        {
            Resources.LoadAll<GameObject>(Define.FieldObjectsPath).ToList(_fieldObjectList);
        }
        else
        {
            Debug.Log("Can't Load because of field object list is null");
        }
    }

    // * 필드 오브젝트 스폰 스팟 로드 메서드
    private void FieldObjectSpawnSpotLoad()
    {
        if (!NullCheck(_fieldObjectSpawnSpotList))
        {
            Resources.LoadAll<GameObject>(Define.FieldObjectSpawnSpotPath).ToList(_fieldObjectSpawnSpotList);
        }
        else
        {
            Debug.Log("Can't Load because of field object spawn spot list is null");
        }
    }

    private void TreasureBoxOpenEffectLoad()
    {
        _treasureBoxOpenEffectResource = Resources.Load<GameObject>(Define.TreasureOpenEffectPath);
    }

    // * 데미지 텍스트 리소스 로드 메서드
    private void DamageTextResourceLoad()
    {
        if(!NullCheck(_damageTextResourceList))
        {
            Resources.LoadAll<GameObject>(Define.DamageTextPath).ToList(_damageTextResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of DamageText list is null");
        }
    }

    private void SkyBoxResourceLoad()
    {
        if (!NullCheck(_skyBoxResourceList))
        {
            Resources.LoadAll<Material>(Define.SkyBoxPath).ToList(_skyBoxResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of skybox list is null");
        }
    }

    private void JourneyRankResourceLoad()
    {
        if (!NullCheck(_journeyRankResourceList))
        {
            Resources.LoadAll<JourneyRankData>(Define.JourneyRankPath).ToList(_journeyRankResourceList);
        }
        else
        {
            Debug.Log("Can't Load because of journey rank list is null");
        }
    }

    private void ItemSlotResourceLoad()
    {
        if(!NullCheck(_itemSlotList))
        {
            Resources.LoadAll<GameObject>(Define.ItemSlotPath).ToList(_itemSlotList);
        }
    }

    private void EnvironmentResourceLoad()
    {
        _backgroundResource = Resources.Load<GameObject>(Define.BackgroundPath);
    }

    // * UI 리소스 로드 메서드
    private void UIResourceLoad()
    {
        _uiGame = Resources.Load<GameObject>(Define.UIGamePath);
        _systemTextResource = Resources.Load<GameObject>(Define.SystemTextPath);
        _treasureTextResource = Resources.Load<GameObject>(Define.TreasureTextPath);
    }

    // * 팝업 UI 리소스 로드 메서드
    private void PopupUIResourceLoad()
    {
        _popupCanvas = Resources.Load<GameObject>(Define.PopupUICanvasPath);
        _popupPanel = Resources.Load<GameObject>(Define.PopupPanelPath);
        _popupJourneyInfo = Resources.Load<GameObject>(Define.PopupJourneyInfoPanelPath);
        _popupStageInfo = Resources.Load<GameObject>(Define.PopupStageInfoPanelPath);
        _popupNamedMonsterInfo = Resources.Load<GameObject>(Define.PopupNamedMonsterInfoPanelPath);
        _popupStatusPanel = Resources.Load<GameObject>(Define.PopupStatusPanelPath);
        _popupInventoryPanel = Resources.Load<GameObject>(Define.PopupInventoryPanelPath);
        _popupSkillInventory = Resources.Load<GameObject>(Define.PopupSkillInventoryPath);
        _popupMerchantPanel = Resources.Load<GameObject>(Define.PopupMerchantPanelPath);
    }

    private void PlayerVitalResourceLoad()
    {
        _playerVitalCanvas = Resources.Load<GameObject>(Define.PlayerVitalCanvasPath);
        _playerVitalResource = Resources.Load<GameObject>(Define.PlayerVitalPath);
    }

    // * 컷신 리소스 로드 메서드
    private void CutSceneResourceLoad()
    {
        _goblinKingCutScene = Resources.Load<GameObject>(Define.GoblinKingCutScenePath);
    }

    private void CameraResourceLoad()
    {
        _followCam = Resources.Load<GameObject>(Define.FollowCameraPath);
        _cutSceneCam = Resources.Load<GameObject>(Define.CutSceneCameraPath);
    }
    #endregion

    #region InstantiateObjectMethod
    public GameObject GetObject<T>(Vector3 spawnPos, string name, Transform parent = default) where T : MonoBehaviour
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
            GameObject obj = Instantiate(NormalMonsterResourceList[name], spawnPos, Quaternion.identity);
            NormalMonsterController normalMonsterController = obj.GetOrAddComponent<NormalMonsterController>();
            return obj;
        }
        else if(type == typeof(NamedMonsterController))
        {
            GameObject obj = Instantiate(NamedMonsterResourceList[name], spawnPos, Quaternion.identity);
            NamedMonsterController namedMonsterController = obj.GetOrAddComponent<NamedMonsterController>();
            return obj;
        }
        else if(type == typeof(Skill))
        {
            GameObject obj = Instantiate(PlayerSkillResourceList[name], spawnPos, Quaternion.identity);
            return obj;
        }
        else if(type == typeof(DamageTextController))
        {
            GameObject obj = Instantiate(DamageTextResourceList[name], spawnPos, Quaternion.identity);
            return obj;
        }
        else if(type == typeof(MonsterGateController))
        {
            GameObject obj = Instantiate(MonsterGateResource, spawnPos, Quaternion.identity);
            return obj;
        }
        else if(type == typeof(SystemTextController))
        {
            GameObject obj = Instantiate(SystemTextResource, spawnPos, Quaternion.identity);
            return obj;
        }
        else if(type == typeof(ItemSlot))
        {
            GameObject obj = Instantiate(ItemSlotList[name], parent);
            return obj;
        }
        else if(type == typeof(RewardTextController))
        {
            GameObject obj = Instantiate(TreasureTextResource, spawnPos, Quaternion.identity);
            return obj;
        }
        return null;
    }
    #endregion
}

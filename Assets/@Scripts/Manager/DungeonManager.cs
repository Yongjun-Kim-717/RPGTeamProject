using System;
using UnityEngine;
using extension;
using System.Collections;

public class DungeonManager : Singleton<DungeonManager>, IEventSubscriber, IDeactivateObject
{
    private GameObject _dungeonWallFront;
    private GameObject _dungeonWallBack;
    private GameObject _dungeonPortal;

    private DungeonPortalController _portalController;

    public GameObject DungeonWallFront
    {
        get { return _dungeonWallFront; }
    }
    public GameObject DungeonWallBack
    {
        get { return _dungeonWallBack; }
    }
    public GameObject DungeonPortal
    { 
        get { return _dungeonPortal; } 
    }

    #region Action
    private Action _onDungeonEnter;
    private Action _onDungeonClear;
    private Action _onDungeonFail;
    private Action _onDungeonExit;
    public Action OnDungeonEnter { get { return _onDungeonEnter; } }
    public Action OnDungeonClear { get { return _onDungeonClear; } }
    public Action OnDungeonFail { get { return _onDungeonFail; } }
    public Action OnDungeonExit { get { return _onDungeonExit; } }
    #endregion

    #region Initialize
    protected override void Initialize()
    {
        _dungeonWallFront = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonWallBack = Instantiate(ObjectManager.Instance.DungeonWallResource);
        _dungeonPortal = Instantiate(ObjectManager.Instance.DungeonPortalResource);

        _portalController = _dungeonPortal.GetComponent<DungeonPortalController>();
    }
    #endregion

    #region IEventSubscriber
    public void Subscribe()
    {
        PopupUIManager.Instance.OnButtonDungeonEnterClick += EnterDungeon;
    }
    #endregion

    #region IDeactivateObject
    public void Deactivate()
    {
        DungeonWallFront.SetActive(false);
        DungeonWallBack.SetActive(false);
        DungeonPortal.SetActive(false);
    }
    #endregion

    // * 포탈 입장 액션 구독 메서드
    //- 던전 입장 기능
    private void EnterDungeon()
    {
        Debug.Log("던전 입장");
        SetWallDown();
    }

    private void SetWallDown()
    {
        _dungeonWallFront.SetActive(false);
    }

    private void SetWallUp()
    {
        _dungeonWallFront.SetActive(true);
    }
}

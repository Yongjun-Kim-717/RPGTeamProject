using System;
using UnityEngine;
using UnityEngine.UI;
using extension;

// * 팝업 UI 매니저 스크립트
//- 특정 이벤트 발생 시 팝업 UI 생성
public class PopupUIManager : Singleton<PopupUIManager>, IEventSubscriber, IDeactivateObject
{
    private GameObject _canvasPopupUI;
    private GameObject _panelEnterDungeon;

    public Button ButtonEnterDungeon;

    public event Action OnButtonDungeonEnterClick;

    #region Initialize
    protected override void Initialize()
    {
        _canvasPopupUI = Instantiate(ObjectManager.Instance.PopupCanvas);
        _panelEnterDungeon = Instantiate(ObjectManager.Instance.PopupPanel, _canvasPopupUI.transform);

        ButtonEnterDungeon = _panelEnterDungeon.GetComponentInChildren<Button>();
    }
    #endregion

    #region IEventSubscriber
    public void Subscribe()
    {
        ButtonEnterDungeon.onClick.AddListener(OnDungeonEnter);
        //DungeonManager.Instance.DungeonPortal.GetOrAddComponent<DungeonPortalController>().OnPotalEnter += PopupUI;
    }
    #endregion

    #region IDeactivate
    public void Deactivate()
    {
        _panelEnterDungeon.SetActive(false);
    }
    #endregion

    private void PopupUI()
    {
        _panelEnterDungeon.SetActive(true);
    }

    private void OnDungeonEnter()
    {
        OnButtonDungeonEnterClick?.Invoke();
        _panelEnterDungeon.SetActive(false);
    }
}

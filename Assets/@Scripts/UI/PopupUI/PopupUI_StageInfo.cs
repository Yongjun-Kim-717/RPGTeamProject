using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class PopupUI_StageInfo : MonoBehaviour
{
    [SerializeField] private Button _actionButton;
    [SerializeField] private Image _monsterCountBarImage;
    [SerializeField] private TMP_Text _monsterCountText;

    private Image[] _statusImages;
    private Image _currentStatusImage;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _actionButton.onClick.AddListener(PerformDungeonAction);
        FieldManager.Instance.DungeonController.OnNormalMonsterDead += SetMonsterCountBar;
        FieldManager.Instance.DungeonController.OnDungeonExit += SetMonsterCountBarClear;
        StageManager.Instance.OnStageActionChanged += SetStatusImage;
        _statusImages = _actionButton.GetComponentsInChildren<Image>();
        _currentStatusImage = null;
        _statusImages[1].gameObject.SetActive(false);
        _statusImages[2].gameObject.SetActive(false);
        _statusImages[3].gameObject.SetActive(false);
        _statusImages[4].gameObject.SetActive(false);
    }

    private void SetMonsterCountBarClear()
    {
        _monsterCountText.text = "0 %";
        _monsterCountBarImage.fillAmount = 0;
    }
    private void SetMonsterCountBar(float currentDeathCount, float clearDeathCount)
    {
        _monsterCountBarImage.fillAmount = currentDeathCount / clearDeathCount;
        int stageProgress = (int)(currentDeathCount / clearDeathCount * 100);
        _monsterCountText.text = $"{ (stageProgress >= 100 ? 100 : stageProgress) } %";
    }

    // * 버튼 클릭 이벤트 연결 메서드
    //- 클릭 전 버튼 상태에 따라 클릭 시 효과가 달라야 함 
    private void PerformDungeonAction()
    {
        switch (StageManager.Instance.StageActionStatus)
        {
            case Define.StageActionStatus.Challenge:
                StageManager.Instance.StageActionStatus = Define.StageActionStatus.ExitStage;
                StageManager.Instance.IsSpawnNamedMonster = true;
                break;
            case Define.StageActionStatus.AutoChallenge:
                StageManager.Instance.StageActionStatus = Define.StageActionStatus.NotChallenge;
                break;
            case Define.StageActionStatus.NotChallenge:
                StageManager.Instance.StageActionStatus = Define.StageActionStatus.AutoChallenge;
                break;
            case Define.StageActionStatus.GoFinalStage:
                StageManager.Instance.StageActionStatus = Define.StageActionStatus.NotChallenge;
                break;
            case Define.StageActionStatus.ExitStage:
                StageManager.Instance.StageActionStatus = Define.StageActionStatus.NotChallenge;
                // 탈출 이벤트 연결
                break;
            default:
                break;
        }
    }

    private void SetStatusImage(Define.StageActionStatus actionStatus)
    {
        GameObject go = _currentStatusImage?.gameObject;
        switch (actionStatus)
        {
            case Define.StageActionStatus.Challenge:
                _currentStatusImage = _statusImages[2];
                break;
            case Define.StageActionStatus.AutoChallenge:
                _currentStatusImage = _statusImages[1];
                break;
            case Define.StageActionStatus.NotChallenge:
                _currentStatusImage = null;
                break;
            case Define.StageActionStatus.GoFinalStage:
                _currentStatusImage = _statusImages[4];
                break;
            case Define.StageActionStatus.ExitStage:
                _currentStatusImage = _statusImages[3];
                break;
            default:
                break;
        }
        if(_currentStatusImage != null)
            _currentStatusImage.gameObject.SetActive(true);
        if(go != null)
            go.SetActive(false);
    }
}

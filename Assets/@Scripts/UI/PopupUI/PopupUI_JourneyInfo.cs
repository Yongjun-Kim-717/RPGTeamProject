using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_JourneyInfo : MonoBehaviour
{
    [SerializeField] TMP_Text _rankText;
    [SerializeField] TMP_Text _journeyExpText;
    [SerializeField] TMP_Text _minJourneyExpText;
    [SerializeField] TMP_Text _maxJourneyExpText;
    [SerializeField] Image _rankImage;
    [SerializeField] Image _journeyGaugeBarImage;

    PlayerData _currentPlayerData;      //플레이어 데이터
    JourneyRankData _journeyRankData;   //플레이어 데이터의 여정 랭크 데이터
    float _currentJourneyExp;
    int _currentJourneyRank;

    bool _isRankChanged = false;

    private void Awake()
    {
        Initialize();
    }
    private void OnEnable()
    {
        PlayerManager.Instance.Player.OnJourneyExpChanged += UpdateJourneyExp;
        PlayerManager.Instance.Player.OnJourneyRankChanged += UpdateJourneyRank;
    }

    private void OnDisable()
    {
        if(PlayerManager.Instance.Player != null)
        {
            PlayerManager.Instance.Player.OnJourneyExpChanged -= UpdateJourneyExp;
            PlayerManager.Instance.Player.OnJourneyRankChanged -= UpdateJourneyRank;
        }
    }
    void Initialize()
    {
        _journeyRankData = PlayerManager.Instance.Player.PlayerData.JourneyRankData;
        _currentPlayerData = PlayerManager.Instance.Player.PlayerData;
        _currentJourneyExp = _currentPlayerData.JourneyExp;
        _currentJourneyRank = _journeyRankData.Index;

        _journeyGaugeBarImage.fillAmount = (_currentJourneyExp - _journeyRankData.MinJourneyExp) / _journeyRankData.MaxJourneyExp;
        _journeyExpText.text = _currentJourneyExp.ToString(); //모험 게이지
        _journeyExpText.color = Color.white;
        UpdateJourneyUI();
    }

    // * 모험 게이지를 업데이트 하는 함수
    void UpdateJourneyExp(float journeyExp)
    {
        _currentJourneyExp += journeyExp; //증가 EXP량 더해줌
        _journeyExpText.text = _currentJourneyExp.ToString();
        _journeyExpText.color = Color.white;

        //EXP가 점진적으로 차오르도록
        StartCoroutine(StartJourneyExp(_currentJourneyExp));
    }

    // * PlayerController의 OnJourneyRankChanged이벤트 구독하는 함수
    void UpdateJourneyRank(int medal)
    {
        if (_currentJourneyRank != medal)
        {
            StartCoroutine(_rankText.GetComponent<UIEffectsManager>().PerformEffect(0));
            StartCoroutine(_rankImage.GetComponent<UIEffectsManager>().PerformEffect(0));
            _currentJourneyRank = medal;
            _journeyRankData = Instantiate(ObjectManager.Instance.JourneyRankResourceList[medal.ToString()]);
            _isRankChanged = true;
        }
    }

    //랭크 변경 시 실행되는 함수
    void UpdateJourneyUI()
    {
        _rankText.text = _journeyRankData.Name;
        _rankText.color = _journeyRankData.TextColor;
        _maxJourneyExpText.text = _journeyRankData.MaxJourneyExp.ToString();
        _rankImage.sprite = _journeyRankData.RankImage;
        _journeyGaugeBarImage.color = _journeyRankData.TextColor;
    }

    IEnumerator StartJourneyExp(float journeyExp)
    {
        float t = 0;
        float end = (journeyExp - _journeyRankData.MinJourneyExp)
            / (_journeyRankData.MaxJourneyExp-_journeyRankData.MinJourneyExp);
        float start = _journeyGaugeBarImage.fillAmount;
        while (Mathf.Abs(_journeyGaugeBarImage.fillAmount - end) > 0.001f)
        {
            t += Time.deltaTime * (end - _journeyGaugeBarImage.fillAmount) * 20;
            _journeyGaugeBarImage.fillAmount = Mathf.Lerp(start, end, t);
            yield return null;
        }
        if (_isRankChanged)
        {
            _isRankChanged = false;
            _journeyGaugeBarImage.fillAmount = 0;
            StartCoroutine(_rankText.GetComponent<UIEffectsManager>().PerformEffect(1));
            StartCoroutine(_rankImage.GetComponent<UIEffectsManager>().PerformEffect(1));
            UpdateJourneyUI();
        }
    }
}

using NUnit.Framework;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupUI_GainRecord : MonoBehaviour
{
    [SerializeField] List<TMP_Text> _goodsCountTextList;

    [SerializeField] TMP_Text _timerText;
    [SerializeField] TMP_Text _deadMonsterText;

    [SerializeField] Button _exitButton;
    [SerializeField] Button _resetButton;

    int _killCount;
    int _silverCoin;
    float _exp;
    int _enhancementStone;

    float _time;
    int _minute;
    int _second;

    public event Action OnExitButtonClicked;

    private void Awake()
    {
        _exitButton.onClick.AddListener(OnExitButtonClick);
        _resetButton.onClick.AddListener(OnResetButtonClick);
    }

    void OnExitButtonClick()
    {
        OnExitButtonClicked?.Invoke();
    }

    void OnResetButtonClick()
    {
        _killCount = 0;
        _silverCoin = 0;
        _exp = 0;
        _enhancementStone = 0;
        _time = -1;

        SetTime();
        SetText();
    }

    public void SetTime()
    {
        _time += 1;
        _minute = (int)_time / 60;
        _second = (int)_time % 60;
        _timerText.text = _minute.ToString("00") + " : " + _second.ToString("00");
    }

    public void SetGoods(Define.GoodsType type, float amount)
    {
        switch (type)
        {
            case Define.GoodsType.SilverCoin:
                _silverCoin += (int)amount;
                break;
            case Define.GoodsType.Exp:
                _exp += amount;
                break;
            case Define.GoodsType.EnhancementStone:
                _enhancementStone += (int)amount;
                break;
        }
        if(type != Define.GoodsType.None)
            _killCount++;

        SetText();
    }

    void SetText()
    {
        _deadMonsterText.text = _killCount.ToString();
        _goodsCountTextList[0].text = _silverCoin.ToString();
        _goodsCountTextList[1].text = _exp.ToString();
        _goodsCountTextList[2].text = _enhancementStone.ToString();
    }
}

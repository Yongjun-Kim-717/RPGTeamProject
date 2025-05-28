using TMPro;
using UnityEngine;

public class UI_Game : MonoBehaviour
{
    [SerializeField] TMP_Text _timeText;
    private int _minute;
    private int _second;

    private void OnEnable()
    {
        TimeManager.Instance.OnTimeChanged += SetTimeText;
    }

    private void OnDisable()
    {
        TimeManager.Instance.OnTimeChanged -= SetTimeText;
    }

    void SetTimeText(float time)
    {
        _minute = (int)time / 60;
        _second = (int)time % 60;
        _timeText.text = _minute.ToString("00") + " : " + _second.ToString("00");
    }
}

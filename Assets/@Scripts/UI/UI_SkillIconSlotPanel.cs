using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_SkillIconSlotPanel : MonoBehaviour
{
    [SerializeField] GameObject _skillIconSlot;
    SkillIconSlot[] _iconSlots = new SkillIconSlot[6];
    const int _numOfColumn = 3;
    readonly Vector2 _start = new Vector2(-170, 70);
    readonly Vector2 _space = new Vector2(160, 150);
    readonly Vector2 _size = new Vector2(160, 160);

    private void Awake()
    {
        CreateSkillIconSlots();
        SkillManager.Instance.SetIconSlots(_iconSlots);
        SkillManager.Instance.LockIconSlots(PlayerManager.Instance.Player.PlayerData.UnlockedSkillSlotCount);
        PlayerManager.Instance.SkillSystem.SetSkillSlotList();
    }

    void CreateSkillIconSlots()
    {
        for (int i = 0; i < _iconSlots.Length; i++)
        {
            GameObject go = Instantiate(_skillIconSlot);
            go.name = $"SkillIonSlot " + i;
            go.transform.SetParent(transform);
            RectTransform rect = go.GetComponent<RectTransform>();
            rect.anchoredPosition = CalculateSlotPosition(i);
            rect.sizeDelta = _size;
            //Debug.Log($"{i}: {rect.localPosition}");
            AddEvent(go, EventTriggerType.PointerClick, (data) => { OnClick(go, (PointerEventData)data); });
            _iconSlots[i] = go.GetComponent<SkillIconSlot>();
        }
    }

    Vector2 CalculateSlotPosition(int idx)
    {
        float posX = _start.x + ((_space.x) * (idx % _numOfColumn));
        float posY = _start.y - ((_space.y) * (idx / _numOfColumn));
        return new Vector2(posX, posY);
    }

    #region Events
    void AddEvent(GameObject go, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        var trigger = go.GetComponent<EventTrigger>();
        if (!trigger) return;
        EventTrigger.Entry eventTrigger = new EventTrigger.Entry { eventID = type };
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnLeftClick(SkillIconSlot slot)
    {
        //해당 아이콘의 스킬 실행
        slot.OnClickSkillIcon?.Invoke();
    }

    public void OnClick(GameObject go, PointerEventData data)
    {
        SkillIconSlot slot = go.GetComponent<SkillIconSlot>();
        if (slot == null)
            return;
        if (data.button == PointerEventData.InputButton.Left)
        {
            OnLeftClick(slot);
        }
    }
    #endregion
}

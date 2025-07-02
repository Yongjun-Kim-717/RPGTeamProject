using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //플레이어가 보유 중인 스킬 리소스들을 담은 리스트
    List<Skill> _skillList = new List<Skill>();

    // 스킬 슬롯 리스트에 실질적으로 들어있는 스킬 개수 확인용
    int _skillSlotCount = 0;
    // 스킬 슬롯 리스트 - 액티브형 스킬 보관 슬롯
    List<SkillSlot> _activeSkillSlotList = new List<SkillSlot>();
    BasicSkillSlot _basicSkillSlot;

    PlayerController _player;

    public Action<float> OnShortestSkillDistanceChanged;

    public bool IsAuto { get; set; }

    public List<Skill> SkillList { get { return _skillList; } }

    public BasicSkillSlot BasicSkillSlot
    {
        get { return _basicSkillSlot; }
    }

    public void InitializeSkillSystem()
    {
        _player = PlayerManager.Instance.Player;
        OnShortestSkillDistanceChanged += _player.SetShortestSkillDistance;

        // 오브젝트매니저가 보유 중인 스킬 프리팹들을 플레이어의 스킬리스트에 넣기
        Dictionary<string, GameObject> skillList = ObjectManager.Instance.PlayerSkillResourceList;
        SkillData[] skillDatas = _player.PlayerData.CurrentSkillData;
        foreach (var skillData in skillDatas)
        {
            _skillList.Add(skillList[skillData.name].GetComponent<Skill>());
        }
    }

    private void Update()
    {
        if (IsAuto)
        {
            // 기본 공격할 타이밍인지 체크
            if (IsBasicAttackPossible())
            {
                _basicSkillSlot.ActivateSlotSkill();
            }
            else
            {
                foreach (var slot in _activeSkillSlotList)
                {
                    if (!SkillManager.Instance.IsSkillInterval && slot != null)
                    {
                        slot.ActivateSlotSkill();
                    }
                }
            }
        }
    }

    bool IsBasicAttackPossible()
    {
        // skill interval이거나
        // 모든 스킬 슬롯이 쿨타임 중이거나 스킬이 없을 때
        return SkillManager.Instance.IsSkillInterval || _activeSkillSlotList.All(slot => !slot || !slot.IsActivatePossible);
    }

    public void SetSkillSlotList()
    {
        // 플레이어가 현재 보유 중인 스킬 중에서 열린 슬롯 개수만큼 가져와야함
        foreach (var skill in _skillList)
        {
            AddSkill(skill.SkillData);
        }
    }

    public void AddSkill(SkillData data)
    {
        // 이미 스킬 아이콘 슬롯에 보유 중인 스킬인지 검사
        SkillSlot skillSlot = _activeSkillSlotList.Find((slot) => slot.SkillData == data);
        if (skillSlot != null)
        {
            Debug.Log($"Skill named {data.SkillName} already exists!!!");
            return;
        }

        GameObject skillResource = ObjectManager.Instance.PlayerSkillResourceList[data.name];
        // 패시브면 효과 적용만 시키고, 슬롯은 x
        if (skillResource.GetComponent<PassiveSkill>() != null)
        {
            PassiveSkill passive = Instantiate(skillResource).GetComponent<PassiveSkill>();
            passive.Initialize(_player.PlayerData);
        }
        // 액티브면 슬롯 만들어서 저장 및 관리
        else
        {
            GameObject go = new GameObject(data.name + " slot");
            go.transform.SetParent(transform);
            go.transform.localPosition = Vector3.up;

            // 기본 공격이면 기본공격 슬롯 따로 만들어서 저장 및 관리
            if (data.name == "PlayerBasicAttack")
            {
                _basicSkillSlot = go.AddComponent<BasicSkillSlot>();
                _basicSkillSlot.SetSkill(data);
                OnShortestSkillDistanceChanged?.Invoke(data.TargetDistance);
            }

            else
            {
                // 이미 스킬 아이콘 슬롯 자리 다 찼으면 슬롯 생성 x
                if (_skillSlotCount == _player.PlayerData.UnlockedSkillSlotCount)
                {
                    Debug.Log("Slot list is already full!!!");
                    Destroy(go);
                    return;
                }

                SkillSlot slot = go.AddComponent<SkillSlot>();
                // 비어있는 인덱스 있나 찾아보고
                int idx = _activeSkillSlotList.FindIndex((slot) => slot == null);

                // 비어있는 인덱스 없는 경우(idx == -1)
                if (idx < 0)
                {
                    _activeSkillSlotList.Add(slot);
                    SkillManager.Instance.SubscribeEvents(slot, _activeSkillSlotList.Count - 1);
                    slot.OnActivateSkill += StartSkillInterval;
                }
                else
                {
                    _activeSkillSlotList[idx] = slot;
                    SkillManager.Instance.SubscribeEvents(slot, idx);
                    slot.OnActivateSkill += StartSkillInterval;
                }
                if(slot.SetSkill(data))
                {
                    _skillSlotCount++;
                }
            }
        }

    }

    public void RemoveSkill(SkillData data)
    {
        SkillSlot slot = _activeSkillSlotList.Find((slot) => slot.SkillData == data);
        if (slot == null)
        {
            Debug.Log("Cannot Find Skill with Name " + data.SkillName);
            return;
        }
        slot.DestroySkillSlot();
        _skillSlotCount--;
    }

    public float GetShortestSkillDistance()
    {
        float min = -1;
        foreach (var slot in _activeSkillSlotList)
        {
            if (slot != null)
            {
                if (min < 0)
                {
                    min = slot.SkillData.TargetDistance;
                }
                else
                {
                    min = Mathf.Min(min, slot.SkillData.TargetDistance);
                }
            }
        }
        if (min < 0)
        {
            min = BasicSkillSlot.SkillData.TargetDistance;
        }

        OnShortestSkillDistanceChanged?.Invoke(min);
        return min;
    }

    void StartSkillInterval()
    {
        StartCoroutine(CoSkillInterval());
    }

    IEnumerator CoSkillInterval()
    {
        SkillManager.Instance.IsSkillInterval = true;
        yield return new WaitForSeconds(Define.SkillInterval);
        SkillManager.Instance.IsSkillInterval = false;
    }
}

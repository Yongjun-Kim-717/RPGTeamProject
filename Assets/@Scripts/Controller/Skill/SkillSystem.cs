using System;
using System.Collections.Generic;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //플레이어의 스킬 리스트 - 스킬 슬롯
    [SerializeField] List<Skill> _skillList = new List<Skill>();

    // 스킬 슬롯 리스트 - 액티브형 스킬 보관 슬롯
    List<SkillSlot> _slotList = new List<SkillSlot>();

    private void Awake()
    {
        InitializeSkillSystem();
    }

    void InitializeSkillSystem()
    {
        foreach (var skill in _skillList)
        {
            // 패시브면 효과 적용만 시키고
            // 액티브면 슬롯 만들어서 저장 및 관리
            GameObject go = new GameObject(skill.name + " slot");
            go.transform.SetParent(transform);
            go.transform.localPosition = new Vector3(0, 0.5f, 0);
            SkillSlot slot = go.AddComponent<SkillSlot>();
            slot.SetSkill(skill);
            _slotList.Add(slot);
        }

    }

    public void TestAttack()
    {
        //_skillList[0]?.StartAttack();
        //SkillAttack();
    }


    //#region Skill Queue
    //Queue<GameObject> _skillQueue = new Queue<GameObject>();
    //Dictionary<Skill, GameObject> _skillDictionary = new Dictionary<Skill, GameObject>();

    //[SerializeField] SkillData[] skillList;
    //public static Action<GameObject> OnEnqueueSkill;
    //public static Action<SkillTest> OnSkillCoolTime;

    //void InitializeSkill()
    //{
    //    OnEnqueueSkill += EnqueueSkill;
    //    //skillList = Resources.LoadAll<SkillTest>("");
    //    for (int i = 0; i < skillList.Length; i++)
    //    {
    //        var skillObject = Instantiate(skillList[i], transform).skillPrefab;
    //        skillObject.GetComponent<Skill>()?.Initialize(skillList[i]);
    //        _skillQueue.Enqueue(skillObject);
    //    }
    //}

    //void EnqueueSkill(GameObject skill)
    //{
    //    _skillQueue.Enqueue(skill);
    //}

    //void SkillAttack()
    //{
    //    //if (_skillQueue.Count > 0 && !_animator.GetBool(Define.IsAttacking))
    //    if(_skillQueue.Count > 0)
    //    {
    //        _skillQueue.Dequeue().GetComponent<Skill>().StartAttack();
    //    }
    //}

    //public void AddDictionary(GameObject skill)
    //{
    //    _skillDictionary.Add(skill.GetComponent<Skill>(), skill);
    //}

    //public void RemoveDictionary(GameObject skill)
    //{
    //    _skillDictionary.Remove(skill.GetComponent<Skill>());
    //    _skillQueue.Enqueue(skill);
    //}
    //#endregion
}

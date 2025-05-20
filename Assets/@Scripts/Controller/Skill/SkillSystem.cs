using System;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ������ ��ũ��Ʈ
public class SkillSystem : MonoBehaviour
{
    //��ų ������ ����Ʈ
    [SerializeField] List<SkillData> skillDataList;

    //�÷��̾��� ��ų ����Ʈ - ��ų ����
    List<Skill> _skillList = new List<Skill>();

    // ��ų ���� ����Ʈ
    List<SkillSlot> _slotList = new List<SkillSlot>();

    private void Start()
    {
        // skillDataList���� �����͸� ������ ��ų ���Կ� ����Ѵ�
        foreach(var data in skillDataList)
        {
            if (data.skillPrefab != null)
            {
                GameObject gameObject=new GameObject(data.skillPrefab.name);
                gameObject.transform.SetParent(transform);
                SkillSlot slot = gameObject.AddComponent<SkillSlot>();
                slot.SetSkill(data);
                _slotList.Add(slot);

                ////data�� �ش��ϴ� ��ų �������� ����
                //GameObject skillObject = Instantiate(data.skillPrefab, transform);
                //skillObject.SetActive(false);
                //Skill skillComponent = skillObject.GetComponent<Skill>();
                //if (skillComponent != null)
                //{
                //    skillComponent.Initialize(data);
                //    _skillList.Add(skillComponent);
                //}
            }
        }
        //InitializeSkill();
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

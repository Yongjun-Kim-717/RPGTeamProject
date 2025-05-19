using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾ ������ ��ũ��Ʈ
public class SkillSystem : MonoBehaviour
{
    //��ų ������ ����Ʈ
    public List<SkillData> skillDataList;

    //�÷��̾��� ��ų ����Ʈ
    List<Skill> _skillList = new();

    private void Start()
    {
        foreach(var data in skillDataList)
        {
            if (data.skillPrefab != null)
            {
                //data�� �ش��ϴ� ��ų �������� ����
                GameObject skillObject = Instantiate(data.skillPrefab, transform);
                skillObject.SetActive(false);
                Skill skillComponent = skillObject.GetComponent<Skill>();
                if (skillComponent != null)
                {
                    skillComponent.Initialize(data);
                    _skillList.Add(skillComponent);
                }
            }
        }
    }
}

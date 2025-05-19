using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어에 부착할 스크립트
public class SkillSystem : MonoBehaviour
{
    //스킬 데이터 리스트
    public List<SkillData> skillDataList;

    //플레이어의 스킬 리스트
    List<Skill> _skillList = new();

    private void Start()
    {
        foreach(var data in skillDataList)
        {
            if (data.skillPrefab != null)
            {
                //data에 해당하는 스킬 프리팹을 복제
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

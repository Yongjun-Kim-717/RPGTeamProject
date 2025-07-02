using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(EventTrigger))]
public class SkillItemSlot : MonoBehaviour
{
    [SerializeField] Image _skillIconImage;
    [SerializeField] TMP_Text _skillName;
    SkillData _skillData;

    // 마우스 클릭 시 스킬 설명 UI 등장

    public SkillData SkillData
    {
        get { return _skillData; }
    }

    public Action<SkillItemSlot> OnPostUpdate;

    public void UpdateSlot(SkillData skillData = null)
    {
        bool isExist = skillData ? true : false;
        _skillData = skillData;
        _skillIconImage.sprite = isExist ? skillData.SkillIcon : null;
        _skillIconImage.color = isExist ? new Color(1, 1, 1, 1) : new Color(0, 0, 0, 0);
        _skillName.text = isExist ? skillData.SkillName : string.Empty;
    }

    private void Awake()
    {
        _skillIconImage.color = new Color(0, 0, 0, 0);
        _skillName.text = string.Empty;
    }
}

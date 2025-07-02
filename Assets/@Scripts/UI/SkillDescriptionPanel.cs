using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillDescriptionPanel : MonoBehaviour
{
    [SerializeField] TMP_Text _skillName;
    [SerializeField] TMP_Text _skillDescription;
    [SerializeField] Image _skillIcon;
    [SerializeField] Button _equipButton;
    [SerializeField] Button _releaseButton;
    [SerializeField] Button _exitButton;
    SkillData _skillData;

    public Action<SkillData> OnEquipSkill;
    public Action<SkillData> OnReleaseSkill;

    private void Awake()
    {
        _equipButton.onClick.AddListener(OnEquipButtonClick);
        _releaseButton.onClick.AddListener(OnReleaseButtonClick);
        _exitButton.onClick.AddListener(OnExitButtonClick);
        gameObject.SetActive(false);
    }

    public void TurnOnDescription(SkillData skillData)
    {
        if (skillData == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        _skillData = skillData;
        _skillName.text = skillData.SkillName;
        _skillDescription.text = skillData.SkillDescription;
        _skillIcon.sprite = skillData.SkillIcon;
        // 패시브 스킬은 스킬 아이콘 슬롯에 등록 x
        if(skillData.IsPassive)
        {
            _equipButton.gameObject.SetActive(false);
            _releaseButton.gameObject.SetActive(false);
        }
        else
        {
            _equipButton.gameObject.SetActive(true);
            _releaseButton.gameObject.SetActive(true);
        }
    }

    void OnEquipButtonClick()
    {
        // 현재 스킬 슬롯에 이미 있는 스킬이면 경고문?
        // 현재 스킬 슬롯에 없는 스킬이면 장착
        OnEquipSkill?.Invoke(_skillData);
    }

    void OnReleaseButtonClick()
    {
        // 현재 스킬 슬롯에 있는 스킬이면 해제
        // 현재 스킬 슬롯에 없는 스킬이면 경고문?
        OnReleaseSkill?.Invoke(_skillData);
    }

    void OnExitButtonClick()
    {
        gameObject.SetActive(false);
    }
}

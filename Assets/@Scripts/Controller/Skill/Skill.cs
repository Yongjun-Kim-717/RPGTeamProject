using System.Collections;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    [SerializeField] protected WaitForSeconds _skillCoolTime;
    [SerializeField] protected WaitForSeconds _skillDurationTime;

    protected PlayerController _player;

    public virtual void Initialize(SkillData data)
    {
        _skillData = data;
        _skillCoolTime = new WaitForSeconds(data.coolTime);
        _skillDurationTime = new WaitForSeconds(data.durationTime);
        _player = FindAnyObjectByType<PlayerController>();
    }

    //������ ��ų Ȱ��ȭ
    protected abstract void ActivateSkill(Transform target);

    //��ų ���� �� �ش� ��ų ��Ȱ��ȭ
    protected IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }

    //test
    public void StartAttack(Transform target)
    {
        ActivateSkill(target);
    }
}

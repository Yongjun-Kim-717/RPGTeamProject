using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    [SerializeField] protected WaitForSeconds _skillCoolTime;
    [SerializeField] protected WaitForSeconds _skillDurationTime;

    public virtual void Initialize(SkillData data)
    {
        _skillData = data;
        _skillCoolTime = new WaitForSeconds(data.coolTime);
        _skillDurationTime = new WaitForSeconds(data.durationTime);

        StartCoroutine(SkillRoutine());
    }

    //��ų ��Ÿ�� �ڷ�ƾ
    IEnumerator SkillRoutine()
    {
        //���ѷ���
        //���߿� break�� �� ���ؾ� ��
        while (true)
        {
            yield return _skillCoolTime; //��ų ��Ÿ���� �� ����
            ActivateSkill(); //��ų �ߵ�
        }
    }


    //������ ��ų Ȱ��ȭ
    protected abstract void ActivateSkill();

    //��ų ���� �� �ش� ��ų ��Ȱ��ȭ
    protected IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}

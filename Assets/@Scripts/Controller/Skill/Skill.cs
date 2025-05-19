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

    //스킬 쿨타임 코루틴
    IEnumerator SkillRoutine()
    {
        //무한루프
        //나중에 break할 거 정해야 함
        while (true)
        {
            yield return _skillCoolTime; //스킬 쿨타임이 다 차면
            ActivateSkill(); //스킬 발동
        }
    }


    //실제로 스킬 활성화
    protected abstract void ActivateSkill();

    //스킬 시전 후 해당 스킬 비활성화
    protected IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}

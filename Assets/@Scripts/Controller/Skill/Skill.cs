using System.Collections;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected SkillData _skillData;
    protected WaitForSeconds _skillCoolTime;
    protected WaitForSeconds _skillDurationTime;

    public SkillData SkillData { get { return _skillData; } }

    public virtual void Initialize()
    {
        _skillCoolTime = new WaitForSeconds(_skillData.coolTime);
        _skillDurationTime = new WaitForSeconds(_skillData.durationTime);
    }

    public abstract void ActivateSkill(Transform target = null, Vector3 pos = default);

    protected IEnumerator DeActivateSkill()
    {
        yield return _skillDurationTime;
        gameObject.SetActive(false);
    }
}

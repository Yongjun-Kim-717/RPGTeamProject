using UnityEngine;

public class TransformTargetSkill : TargetSkill
{
    SkillColliderController _coll;
    bool _isCasting = false;
    float _currenTime = 0f;

    void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        if (_skillData.castingTime > 0)
        {
            _isCasting = true;
        }
    }
    public override void ActivateSkill(Transform target, Vector3 pos = default)
    {
        base.ActivateSkill(target, pos);
        _coll.transform.localPosition = Vector3.zero;
    }

    public override void Initialize()
    {
        base.Initialize();
        _coll = GetComponentInChildren<SkillColliderController>();
        _coll.SetColliderInfo(_skillData.damage, _skillData.hitEffectPrefab);
    }
    private void Update()
    {
        if (!_isCasting)
        {
            if(Vector3.Distance(transform.position, _coll.transform.position) < _skillData.targetDistance)
            {
                _coll.transform.Translate(_direction * _skillData.speed * Time.deltaTime, Space.World);
            }
        }
        else
        {
            _currenTime += Time.deltaTime;
            if (_currenTime >= _skillData.castingTime)
            {
                _isCasting = false;
                _currenTime = 0;
            }
        }
    }


}

using System;
using System.Collections;
using UnityEditor;
using UnityEngine;

public class HolySwordSkill : AreaTargetSkill, IRotationSkill, IDirectionSkill, IUltimateSkill
{
    [SerializeField] GameObject _lastExplosion;
    ParticleSystem _particle;
    Animator _animator;
    int _animationHash;
    protected Vector3 _direction;

    public event Action<Vector3> OnActivateSkill;

    public override void Initialize(Status status)
    {
        base.Initialize(status);
        InitializeAnimationSetting();
    }

    public override bool ActivateSkill(Vector3 pos)
    {
        if (base.ActivateSkill(pos))
        {
            // 눈에서 안 보이게 꼼수
            _particle.Stop();
            _lastExplosion.SetActive(false);
            _coll.gameObject.SetActive(false);
            StartCoroutine(CoActivateSkillwithMotion(pos));

            return true;
        }
        return false;
    }

    public IEnumerator CoActivateSkillwithMotion(Vector3 pos)
    {
        // 해당 궁극기의 모션 실행해주고
        _animator.SetTrigger(_animationHash);
        SetDirection();
        OnActivateSkill?.Invoke(_direction);
        // 특정 타이밍에 스킬 실행
        yield return new WaitUntil(() => _animator.GetBool(Define.UltimateReady));
        _particle.Play();
        _coll.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        ActivateLastExplosion();
    }

    public void InitializeAnimationSetting()
    {
        _animator = _player.GetComponent<Animator>();
        _animationHash = Animator.StringToHash(_skillData.SkillAnimationName);
        _particle = GetComponent<ParticleSystem>();
    }

    void ActivateLastExplosion()
    {
        _lastExplosion.SetActive(true);
    }

    public void SetDirection()
    {
        Vector3 dir = _target.position - _player.transform.position;
        dir.y = 0;
        _direction = dir.normalized;
    }
}

using UnityEngine;

public class ActiveSkill : Skill
{
    protected Animator _animator;

    // 스킬 오브젝트 활성화 + 모션 실행 + 비활성화 코루틴
    public override void ActivateSkill(Transform target = null, Vector3 pos = default)
    {
        //플레이어 위치에 스킬 활성화
        gameObject.SetActive(true);
        transform.position = pos;
        // particle system인 경우
        ParticleSystem particleSystem = gameObject.GetComponent<ParticleSystem>();
        if (particleSystem != null)
            particleSystem.Play();

        //스킬 시전 후 스킬 비활성화
        StartCoroutine(DeActivateSkill());
    }

    public override void Initialize()
    {
        base.Initialize();
    }
}
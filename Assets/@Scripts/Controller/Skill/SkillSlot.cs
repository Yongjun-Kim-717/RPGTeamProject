using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// SkillSlot
/// Target�� ��ų���� ���� �� ��ų ��Ÿ�� �����ϴ� ����
/// NonTarget�� ��ų���� ��Ǹ� �˾Ƽ� �ߵ��ǹǷ� �ʿ������(�ϴ���)
public class SkillSlot : MonoBehaviour
{
    SkillData _skillData;
    ActiveSkill _currentSkill;
    PlayerController _player;
    Transform _target;

    // ���Կ� ��ϵ� ��ų�� ��� ���� ����
    public bool IsActivatePossible { get; set; }

    private void Awake()
    {
        // ���߿� ���ӸŴ������� ���������� �� ����
        _player = FindAnyObjectByType<PlayerController>();
        IsActivatePossible = false;
    }

    // ó�� ���� ���� �� ��ų ���
    public void SetSkill(SkillData skillData)
    {
        // �� ó���� ��� ������ ����
        IsActivatePossible = true;
        _skillData = skillData;
        _currentSkill = Instantiate(_skillData.skillPrefab, Vector3.zero, Quaternion.identity, transform).GetComponent<ActiveSkill>();
        _currentSkill.Initialize(skillData);
        // ��ų ������Ʈ �����Ǹ� Ȱ��ȭ
        IsActivatePossible = true;
    }

    public IEnumerator CoStartCoolTime()
    {
        yield return new WaitForSeconds(_skillData.coolTime);
        IsActivatePossible = true;
    }

    private void Update()
    {
        // ��Ÿ�� �ʱ�ȭ�Ǿ� ��ų ��� ������ ���
        if (IsActivatePossible)
        {
            // ���� ����� Ÿ���� Ž���ϰ�, ������ ��ų �ߵ�
            _target = GetNearestTarget(_skillData.targetDistance)?.transform;
            if (_target != null)
            {
                _currentSkill.StartAttack(_target);
                IsActivatePossible = false;
                StartCoroutine(CoStartCoolTime());
            }
        }
    }

    GameObject GetNearestTarget(float distance)
    {
        if (_player == null)
            _player = FindAnyObjectByType<PlayerController>();
        //�Ÿ� ���� monster collider Ž��
        Collider[] targets = Physics.OverlapSphere(_player.transform.position, distance, 1 << LayerMask.NameToLayer(Define.MonsterTag));
        if (targets == null)
            return null;
        HashSet<Collider> neighbors = new HashSet<Collider>(targets);

        //�Ÿ� ������ �����Ͽ� ���� ����� ���� ��ȯ
        var neighbor = neighbors.OrderBy(coll => (_player.transform.position - coll.transform.position).sqrMagnitude).FirstOrDefault();
        if (neighbor == null)
            return null;

        return neighbor.gameObject;
    }
}

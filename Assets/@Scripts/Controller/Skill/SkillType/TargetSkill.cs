using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Ÿ���� ��ų�� ������ ��ũ��Ʈ
public class TargetSkill : ActiveSkill
{
    protected override void ActivateSkill(Transform target)
    {

        //���� �� ���� ����� ���� ã�´�
        //Transform target = GetNearestTarget(_skillData.targetDistance)?.transform;

        if (target != null)
        {
            //Ÿ�� �������� ��ų ���� ����
            Vector3 dir = (target.position - _player.transform.position).normalized;

            //�÷��̾� ��ġ�� ��ų ����
            //transform.position = _player.transform.position;
            //transform.rotation = Quaternion.LookRotation(dir);
            transform.localPosition = Vector3.zero;
            _player.Rotate(dir);
            gameObject.SetActive(true);
            // particle system�� ���
            gameObject.GetComponent<ParticleSystem>()?.Play();
            StartCoroutine(DeActivateSkill()); //��ų ���� �� ��ų ��Ȱ��ȭ
        }

        //StartCoroutine(DeActivateSkill());
    }

    GameObject GetNearestTarget(float distance)
    {
        //�Ÿ� ���� monster collider Ž��
        Collider[] targets = Physics.OverlapSphere(_player.transform.position, distance, LayerMask.NameToLayer(Define.MonsterTag));
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

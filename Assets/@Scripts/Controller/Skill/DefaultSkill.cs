using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//Ÿ���� ��ų�� ������ ��ũ��Ʈ
public class DefaultSkill : Skill
{
    private PlayerController _player;

    //private HashSet<Enemy> _enemyList;
    protected override void ActivateSkill()
    {

        //���� �� ���� ����� ���� ã�´�
        Transform target = GetNearestTarget(_skillData.targetDistance)?.transform;

        if (target != null)
        {
            //���� ��ũ��Ʈ�� �÷��̾ �پ��ִٰ� ����

            //Ÿ�� �������� ��ų ���� ����
            Vector3 dir = (target.position - transform.position).normalized;

            //�÷��̾� ��ġ�� ��ų ����
            //transform.position = _player.Center;
            transform.rotation = Quaternion.LookRotation(dir);
            gameObject.SetActive(true);
            StartCoroutine(DeActivateSkill()); //��ų ���� �� ��ų ��Ȱ��ȭ
        }
    }

    GameObject GetNearestTarget(float distance)
    {
        //Ȱ��ȭ�� enemy�� ã�� list Ÿ������ ��ȯ
        ////var targetList = _enemyList.Where(enemy => enemy.gameObject.activeSelf).ToList();

        ////�Ÿ� ������ �����Ͽ� ���� ����� ���� ��ȯ
        //var target = targetList.OrderBy(
        //    enemy => (_player.Center - enemy.transform.position).sqrMagnitude).FirstOrDefault();

        //if (target==null || (target.transform.position - _player.Center).sqrMagnitude > distance * distance)
        //    return null;

        //return target.gameObject;
        return null;
    }
}

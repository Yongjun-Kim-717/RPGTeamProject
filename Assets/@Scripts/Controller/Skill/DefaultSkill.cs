using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//타겟형 스킬에 부착될 스크립트
public class DefaultSkill : Skill
{
    private PlayerController _player;

    //private HashSet<Enemy> _enemyList;
    protected override void ActivateSkill()
    {

        //범위 내 가장 가까운 적을 찾는다
        Transform target = GetNearestTarget(_skillData.targetDistance)?.transform;

        if (target != null)
        {
            //현재 스크립트가 플레이어에 붙어있다고 가정

            //타겟 방향으로 스킬 방향 설정
            Vector3 dir = (target.position - transform.position).normalized;

            //플레이어 위치에 스킬 생성
            //transform.position = _player.Center;
            transform.rotation = Quaternion.LookRotation(dir);
            gameObject.SetActive(true);
            StartCoroutine(DeActivateSkill()); //스킬 시전 후 스킬 비활성화
        }
    }

    GameObject GetNearestTarget(float distance)
    {
        //활성화된 enemy를 찾아 list 타입으로 반환
        ////var targetList = _enemyList.Where(enemy => enemy.gameObject.activeSelf).ToList();

        ////거리 순으로 정렬하여 가장 가까운 적을 반환
        //var target = targetList.OrderBy(
        //    enemy => (_player.Center - enemy.transform.position).sqrMagnitude).FirstOrDefault();

        //if (target==null || (target.transform.position - _player.Center).sqrMagnitude > distance * distance)
        //    return null;

        //return target.gameObject;
        return null;
    }
}

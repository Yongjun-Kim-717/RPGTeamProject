using System;
using UnityEngine;

public class MoveRangeController : MonoBehaviour
{
    public Action OnMoveToTarget;
    public Action OnMoveToOrigin;
    private CapsuleCollider _moveRange;

    public void Intiailize(float moveRange)
    {
        transform.position = transform.parent.position;

        _moveRange = gameObject.AddComponent<CapsuleCollider>();
        _moveRange.isTrigger = true;
        _moveRange.radius = moveRange;
        _moveRange.center += Vector3.up;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            Debug.Log("이동 범위 안에 들어옴");
            OnMoveToTarget?.Invoke();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Define.PlayerTag))
        {
            Debug.Log("이동 범위 벗어남");
            OnMoveToOrigin?.Invoke();
        }
    }
}

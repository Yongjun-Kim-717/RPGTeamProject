using UnityEngine;

public class NormalMonsterController : MonsterController
{
    private void Awake()
    {
        base.Initialize();
    }

    private void Update()
    {
        MoveToTarget(_target.transform.position);
    }
}

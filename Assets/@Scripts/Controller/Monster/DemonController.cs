using UnityEngine;

public class DemonController : MonsterController
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

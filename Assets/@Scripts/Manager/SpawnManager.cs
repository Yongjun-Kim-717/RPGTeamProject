using UnityEngine;

// * SpawnManager 스크립트
//- 몬스터 스포너 활성화
//- spawn pos를 지정할까?
public class SpawnManager : Singleton<SpawnManager>, IEventSubscriber
{
    protected override void Initialize()
    {
        base.Initialize();
    }

    public void Subscribe()
    {
        DungeonManager.Instance.OnDungeonEnter += 
    }

    void SetSpawner()
    {

    }
    
}

using UnityEngine;

// * 싱글톤 디자인 스크립트
//- 모든 매니저 스크립트에서 상속
//- 첫 Instance 호출 시 생성 및 부모 연결
//- Manager.Instance.CreateManager()로 생성 및 초기화
public class Singleton<T> : MonoBehaviour where T: MonoBehaviour
{
    protected static T _instance = null;
    //public static bool IsInstance => _instance != null;
    //public static T TryGetInstance() => IsInstance ? _instance : null;
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject manager = GameObject.Find("@Managers");
                if (manager == null)
                {
                    manager = new GameObject("@Managers");
                    DontDestroyOnLoad(manager);
                }
                _instance = FindAnyObjectByType<T>();
                if (_instance == null)
                {
                    GameObject obj = new GameObject(typeof(T).Name);
                    T component = obj.AddComponent<T>();

                    obj.transform.parent = manager.transform;
                    _instance = component;
                }
            }
            return _instance;
        }
    }

    // * 매니저 생성 및 초기화 메서드
    public void CreateManager()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {

    }
}

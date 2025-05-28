using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneController : MonoBehaviour
{
    PlayableDirector _playableDirector;
    [SerializeField] GameObject _virtualCamera;
    [SerializeField] Camera _mainCamera;
    [SerializeField] GameObject _monsterAppearEffectPrefab;
    GameObject _monsterAppearEffect;

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        _playableDirector = GetComponent<PlayableDirector>();
        _playableDirector.stopped += FinishCutScene;
    }

    private void Start()
    {
        PlayCutScene();
        _monsterAppearEffect = Instantiate(_monsterAppearEffectPrefab);
        _monsterAppearEffect.SetActive(false);
    }

    void PlayCutScene()
    {
        //던전 매니저 스크립트를 받아오면
        //Find로 찾는 게 아니라 던전 매니저의 인스턴스에 있는 네임드 몬스터의 스폰 위치를 가져와서
        //사용할 예정(매개변수로) -> 추후 변경 필요
        GameObject monster = GameObject.FindGameObjectWithTag(Define.MonsterTag);
        _monsterAppearEffect.transform.position = monster.transform.position;
        _monsterAppearEffect.SetActive(true);
        _virtualCamera.SetActive(true);
        _playableDirector.Play();
    }

    //컷신이 끝나면 자동으로 호출될 함수
    void FinishCutScene(PlayableDirector pd)
    {
        Destroy(_monsterAppearEffect);
        _virtualCamera.SetActive(false);
        _mainCamera.transform.rotation = Quaternion.Euler(new Vector3(14, 0, 0));
    }
}

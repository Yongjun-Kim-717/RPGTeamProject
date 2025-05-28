using UnityEngine;

//SceneManager
//씬이 변경될 때마다 그 씬에 해당되는 UIManager 생성
//UIManager에서는 해당 UI 오브젝트들을 갖고 있음

public class UIManager : Singleton<UIManager>
{
    [SerializeField] GameObject UI_Game;
    [SerializeField] GameObject UI_Joystick;
    [SerializeField] GameObject UI_CutScene;

    //컷신이 생성이 되면
    //ui 끄기
    void OnActivateCutScene()
    {

    }


}

using UnityEngine;

public class CutSceneManager : Singleton<CutSceneManager>, IDeactivateObject
{
    private GameObject _cutScene;

    protected override void Initialize()
    {
        base.Initialize();
        _cutScene = Instantiate(ObjectManager.Instance.GoblinKingCutScene);
    }

    public void Deactivate()
    {
        _cutScene.SetActive(false);
    }
    public void PlayCutScene()
    {
        _cutScene?.SetActive(true);
        _cutScene.GetComponent<CutSceneController>().PlayCutScene();
    }
}

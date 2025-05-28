using UnityEngine;
using UnityEngine.UI;

public class UI_Base : MonoBehaviour
{
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        SetCanvas();
    }

    private void SetCanvas()
    {
        Canvas canvas = gameObject.GetOrAddComponent<Canvas>();
        if (canvas != null)
        {
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.overrideSorting = true;
        }
        CanvasScaler canvasScaler = gameObject.GetOrAddComponent<CanvasScaler>();

        if (canvasScaler != null)
        {
            canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
        }
    }
}

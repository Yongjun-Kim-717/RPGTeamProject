using UnityEngine;
using UnityEngine.EventSystems;

public class UI_JoyStick : UI_Base, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] GameObject _joyStick;
    [SerializeField] GameObject _handler;
    [SerializeField] float _radius;

    private Vector2 _moveDir;
    private Vector2 _touchPos;
    private Vector2 _originPos;

    GameObject _player;
    protected override void Initialize()
    {
        _originPos = _joyStick.transform.position;
        _radius = _joyStick.GetOrAddComponent<RectTransform>().sizeDelta.y / 3;
        SetActiveJoyStick(false);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        SetActiveJoyStick(true);
        _touchPos = Input.mousePosition;
        _joyStick.transform.position = Input.mousePosition;
        _handler.transform.position = Input.mousePosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 dragPos = eventData.position;
        _moveDir = (dragPos - _touchPos).normalized;
        float distance = (dragPos - _touchPos).sqrMagnitude;

        Vector3 newPos = (distance < _radius) ? _touchPos + (_moveDir * distance) :
            _touchPos + (_moveDir * _radius);
        _handler.transform.position = newPos;
    }


    public void OnPointerUp(PointerEventData eventData)
    {
        _moveDir = Vector2.zero;
        _handler.transform.position = _originPos;
        _joyStick.transform.position = _originPos;
        SetActiveJoyStick(false);
        //_moveDir을 플레이어의 움직이는 방향으로 만들기
    }

    void SetActiveJoyStick(bool isActive)
    {
        _joyStick.SetActive(isActive);
        _handler.SetActive(isActive);
    }

    
}

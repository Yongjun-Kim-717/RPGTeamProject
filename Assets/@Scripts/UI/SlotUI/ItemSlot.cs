using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// * 아이템 슬롯 클래스
[RequireComponent(typeof(EventTrigger))]
public class ItemSlot : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public float longPressedTime = 0.5f;

    private Coroutine _longPressCoroutine;
    private Vector2 _toolTipOffSet = new Vector2(150, 100);
    private bool _isInInventory = false;
    private int _slotIndex;

    [SerializeField] Sprite _itemImage;
    [SerializeField] TMP_Text _countText;

    ItemData _itemData;

    public ItemData ItemData
    {
        get { return _itemData; }
    }

    public void SetItemCount(int count)
    {
        //장비 아이템이 아닌 기타 및 소비 아이템이면
        //아이템의 count 값을 받아와 텍스트로 띄우기
        _countText.text = count.ToString() ;
    }

    // 아이템 슬롯 초기화 필수 메서드
    public void SetData(ItemData itemData, bool isInInventory)
    {
        _isInInventory = isInInventory;
        _itemData = itemData;
        _itemImage = _itemData.IconImage;
        gameObject.transform.GetChild(0).GetComponent<Image>().sprite = _itemImage;
        _countText.enabled = (itemData.Type == Define.ItemType.Equipment) ? false : (itemData.Count != 0 ? true : false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _longPressCoroutine = StartCoroutine(CheckLongPress(eventData.position));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StopLongPress();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopLongPress();
        if(_isInInventory)
        {
            PopupUIManager.Instance.ActivateItemInfoPanel(this);
        }
    }

    private IEnumerator CheckLongPress(Vector2 touchPos)
    {
        yield return new WaitForSeconds(longPressedTime);
        OnToolTip(touchPos);
    }

    // 나영님이 Define으로 색 딕셔너리 옮기시면 아이템 슬롯의 아이템 데이터 별 색으로 4번째 매개변수 전달하기! -> 툴팁의 이름 등급별 색상 변경 적용
    private void OnToolTip(Vector2 touchPos)
    {
        PopupUIManager.Instance.ActivateToolTipPanel(touchPos + _toolTipOffSet, _itemData.Name, _itemData.Description);
    }

    private void OffToolTip()
    {
        PopupUIManager.Instance.DeactivateToolTipPanel();
    }

    private void StopLongPress()
    {
        if (_longPressCoroutine != null)
        {
            StopCoroutine(_longPressCoroutine);
            _longPressCoroutine = null;
        }
        OffToolTip();
    }

    #region ScrollRect
    private ScrollRect _scrollRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _isInInventory = false;
        _scrollRect = transform.parent.parent.parent.GetComponentInParent<ScrollRect>(true);
        StopLongPress(); // 롱프레스 도중 드래그 시작하면 툴팁 끄기
        _scrollRect?.OnBeginDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        _scrollRect?.OnDrag(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _scrollRect?.OnEndDrag(eventData);
        _isInInventory = true;
    }
    #endregion
}

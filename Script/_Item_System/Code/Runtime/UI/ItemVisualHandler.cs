using _Inventory_System_.Code.Runtime.SlotManagment;
using _UI_Managment_.Runtime.Common;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Item_System_.Runtime.UI
{
    public class ItemVisualHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private SlotItem _slotItem;

    private void Start()
    {
        _slotItem = GetComponentInParent<SlotItem>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.DOScale(1.1f, 0.1f);
        UIManager.Instance.ShowInventoryItemInfo(_slotItem.Data, true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.DOScale(Vector3.one, 0.1f);
        UIManager.Instance.ShowInventoryItemInfo(_slotItem.Data, false);
    }
}
}


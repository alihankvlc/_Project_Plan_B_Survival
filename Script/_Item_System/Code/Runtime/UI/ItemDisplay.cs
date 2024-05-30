using _Project_Plan_B_Survival_Item_System.Runtime.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemCountTextMesh;
    [SerializeField] private Image _itemImageContainer;

    public Image ItemImage => _itemImageContainer;
    public TextMeshProUGUI ItemCountTextMesh => _itemCountTextMesh;

    public void UpdateSlotDisplay(ItemData data, int count = 1)
    {
        UpdateItemIcon(data.Icon);
        UpdateItemCount(count);
    }

    public void UpdateItemCount(int count)
    {
        _itemCountTextMesh.SetText(count.ToString());
    }

    public void UpdateItemIcon(Sprite icon)
    {
        _itemImageContainer.sprite = icon;
    }
}

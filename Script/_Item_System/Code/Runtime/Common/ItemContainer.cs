using _Inventory_System_.Code.Runtime.Common;
using _Item_System_.Runtime.Base;
using UnityEngine;
namespace _Item_System_.Runtime.Common
{
    public interface IItemContainer
    {
        public void AddItemToInventory(IItemManagement itemManagment);
    }
    public sealed class ItemContainer : MonoBehaviour, IItemContainer
    {
        [SerializeField] private ItemData _data;
        [SerializeField] private int _count = 1;
        public ItemData Data => _data;

        public void AddItemToInventory(IItemManagement itemManagment)
        {
            itemManagment.AddItemToInventory(_data.Id, _count);
            Destroy(gameObject);
        }
    }
}

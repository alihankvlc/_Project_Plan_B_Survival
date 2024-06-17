using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Item_System_.Runtime.Base;
using _Item_System_.Runtime.Database;
using UnityEngine;
using Zenject;

namespace _Loot_System_.Runtime
{
    public sealed class LootGenerator
    {
        private ItemDatabaseProvider _itemDatabase;
        private ISlotManager _slotManager;

        [Inject]
        private void Constructor(ItemDatabaseProvider itemDatabase, ISlotManager slotManager)
        {
            _itemDatabase = itemDatabase;
        }

        public List<ItemData> GenerateLoot(LootType lootType, Richness richness, int size, ref List<ItemData> items)
        {
            ItemType itemType = GetItemType(lootType);

            List<ItemData> possibleItems = _itemDatabase.GetItemByType(itemType);
            List<ItemData> filteredItems = possibleItems.Where(item =>
                item.RarityType == GetRarityType(richness)).ToList();

            List<ItemData> selectedDatas = filteredItems.OrderBy(_ => Random.value).Take(size).ToList();

            items.AddRange(selectedDatas);
            return items;
        }

        public void Deneme()
        {
            LootingSlot emptySlot = _slotManager.LootingSlots.FirstOrDefault(r =>
                r.Status == SlotStatus.Empty && r.Type == SlotType.Loot);

            GameObject slotItemObject = Object.Instantiate(_slotManager.SlotItem.gameObject, emptySlot.transform);
        }

        private ItemType GetItemType(LootType lootType)
        {
            ItemType type = lootType switch
            {
                LootType.Ammo => ItemType.Ammo,
                LootType.Consumable => ItemType.Consumable,
                LootType.Weapon => ItemType.Weapon,
                _ => ItemType.None
            };

            return type;
        }

        private Rarity GetRarityType(Richness richness)
        {
            Rarity rarity = richness switch
            {
                Richness.Poor => Rarity.Common,
                Richness.Common => Rarity.Uncommon,
                Richness.Uncommon => Rarity.Rare,
                Richness.Epic => Rarity.Epic,
                Richness.Legendary => Rarity.Legendary,
                _ => Rarity.Common
            };

            return rarity;
        }
    }
}
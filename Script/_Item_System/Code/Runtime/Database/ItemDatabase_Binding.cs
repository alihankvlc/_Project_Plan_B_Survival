using UnityEngine;
using Zenject;

namespace _Item_System_.Runtime.Database.Bind
{
    public sealed class ItemDatabase_Binding : MonoInstaller
    {
        [SerializeField] private ItemDatabase _itemDatabase;

        public override void InstallBindings()
        {
            _itemDatabase.Constructor();
            Container.Bind<ItemDatabaseProvider>().To<ItemDatabase>().FromInstance(_itemDatabase).AsSingle();
        }
    }
}
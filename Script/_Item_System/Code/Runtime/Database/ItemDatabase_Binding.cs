using UnityEngine;
using Zenject;

namespace _Project_Plan_B_Survival_Item_System.Runtime.Database
{
    public sealed class ItemDatabase_Binding : MonoInstaller
    {
        [SerializeField] private ItemDatabase _itemDatabase;

        public override void InstallBindings()
        {
            _itemDatabase.Init();
            Container.Bind<ItemDatabaseProvider>().To<ItemDatabase>().FromInstance(_itemDatabase).AsSingle();
        }
    }
}
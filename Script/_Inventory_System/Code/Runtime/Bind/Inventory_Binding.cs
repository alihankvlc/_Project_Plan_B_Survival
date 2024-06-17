using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using _Inventory_System_.Code.Runtime.UI;
using UnityEngine;
using Zenject;

namespace _Inventory_System_.Code.Runtime.Bind
{
    public sealed class Inventory_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Inventory>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<ToolBelt>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<SlotManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<InventoryWindowManager>().FromComponentInHierarchy().AsSingle();

            Container.Bind<SlotInitializer>().AsSingle();
            Container.Bind<InventoryWeight>().AsSingle();
        }
    }
}
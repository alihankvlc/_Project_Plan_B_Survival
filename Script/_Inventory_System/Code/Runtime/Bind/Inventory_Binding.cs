using _Inventory_System_.Code.Runtime.Common;
using _Inventory_System_.Code.Runtime.SlotManagment;
using Zenject;

namespace _Inventory_System_.Code.Runtime.Bind
{
    public sealed class Inventory_Binding : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<IPlayerInventory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SlotHandler>().AsSingle();
            Container.Bind<InventoryWeight>().AsSingle();
        }
    }
}
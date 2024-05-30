using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Common;
using _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Slot_Settings;
using Zenject;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Bind
{
    public class Inventory_Binding : MonoInstaller
    {

        public override void InstallBindings()
        {
            Container.Bind<IPlayerInventory>().FromComponentInHierarchy().AsSingle();
            Container.Bind<SlotHandler>().AsSingle();
            Container.Bind<InventoryWeight>().AsSingle();
        }
    }
}
using UnityEngine;
using Zenject;

namespace _Project_Plan_B_Survival_Inventory_System.Code.Runtime.Bind
{
    public class Inventory_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerInventory>().To<Inventory>().FromComponentInHierarchy().AsSingle();
        }
    }
}
using UnityEngine;
using Zenject;

namespace _Project_Plan_B_Survival_Input_System.Code
{
    public sealed class InputManager_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerInputProvider>().To<InputManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
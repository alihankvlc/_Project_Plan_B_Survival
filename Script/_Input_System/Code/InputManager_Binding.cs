using UnityEngine;
using Zenject;

namespace _Input_System_.Code.Runtime.Bind
{
    public sealed class InputManager_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerInputHandler>().To<InputManager>().FromComponentInHierarchy().AsSingle();
        }
    }
}
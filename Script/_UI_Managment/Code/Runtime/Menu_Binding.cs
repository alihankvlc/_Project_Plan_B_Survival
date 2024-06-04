using Zenject;
using UnityEngine;

namespace _UI_Managment_.Runtime.Common
{
    public sealed class Menu_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IMenuManager>().To<MenuManager>().FromInstance(MenuManager.Instance).AsSingle();
        }
    }
}


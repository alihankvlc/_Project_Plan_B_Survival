using _UI_Managment_.Runtime.Common;
using Zenject;
using UnityEngine;

namespace _UI_Managment_.Runtime.Bind
{
    public class UIManager_Binding : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IVisualHandler>().To<UIManager>().FromInstance(UIManager.Instance).AsSingle();
    }
}
}


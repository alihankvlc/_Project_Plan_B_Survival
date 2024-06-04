using Zenject;
using UnityEngine;

namespace _UI_Managment_.Runtime.Common
{
    public class UIManager_Binding : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IVisualHandler>().To<UIManager>().FromInstance(UIManager.Instance).AsSingle();
    }
}
}


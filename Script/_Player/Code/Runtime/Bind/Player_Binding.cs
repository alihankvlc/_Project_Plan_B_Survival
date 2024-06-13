using _Input_System_.Code.Runtime;
using _Player_System_.Runtime.Combat.Common;
using _Player_System_.Runtime.Common;
using UnityEngine;
using Zenject;

namespace _Player_System_.Runtime.Bind
{
    public class Player_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<IPlayerComponent>().To<PlayerBehaviour>().FromComponentsInHierarchy().AsSingle();
            Container.Bind<IPlayerInputHandler>().To<InputManager>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerCombatListener>().FromComponentInHierarchy().AsSingle();
            Container.BindInterfacesAndSelfTo<PlayerStatHandler>().FromComponentInHierarchy().AsSingle();
        }
    }
}
using _Stat_System.Runtime.Sub;
using _Stat_System.Runtime.Base;
using _Stat_System.Runtime.Common;
using Zenject;
using UnityEngine;

namespace _Stat_System.Runtime.Bind
{
    public class Stat_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<StatManager>().AsSingle();
            Container.Bind<Health>().WithId("PlayerHealth")
                .FromMethod((ctx) => new(ctx.Container.Resolve<StatManager>(), StatGroup.Player)).AsCached();
        }
    }
}


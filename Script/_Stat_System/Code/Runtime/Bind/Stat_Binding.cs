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
            Container.Bind<StatObserverManager>().AsSingle();

            #region Player

            Container.Bind<Health>().WithId(StatGroup.Player)
                .FromMethod((ctx) => new(ctx.Container.Resolve<StatObserverManager>(), StatGroup.Player)).AsCached();
            Container.Bind<Experience>().WithId(StatGroup.Player)
                .FromMethod((ctx) => new(ctx.Container.Resolve<StatObserverManager>(), StatGroup.Player)).AsCached();

            #endregion

            #region Enemy

            Container.Bind<Health>().WithId(StatGroup.Enemy)
                .FromMethod((ctx) => new(ctx.Container.Resolve<StatObserverManager>(), StatGroup.Enemy)).AsTransient();

            #endregion
        }
    }
}
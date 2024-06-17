using System.Collections;
using System.Collections.Generic;
using _Item_System_.Runtime.Database;
using _Loot_System_.Runtime;
using UnityEngine;
using Zenject;

public class Loot_Binding : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<LootGenerator>().AsSingle();
        Container.BindInterfacesAndSelfTo<LootManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<LootWindowManager>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesAndSelfTo<LootSlotManager>().FromComponentInHierarchy().AsSingle();
    }
}
using System.Collections;
using System.Collections.Generic;
using _Crafting_System_.Runtime.UI;
using UnityEngine;
using Zenject;

namespace _Crafting_System_.Runtime.Binding
{
    public class Crafting_Binding : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<CraftingPanelDisplay>().FromComponentInHierarchy().AsSingle();
        }
    }
}

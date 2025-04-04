using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BootstrapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<AssetLoadingDisplay>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<AssetLoader>().AsSingle().NonLazy();
        Container.Bind<MenuLoader>().AsTransient().NonLazy();
        Container.BindInterfacesAndSelfTo<BootstrapInitializer>().AsSingle().NonLazy();
    }    
}

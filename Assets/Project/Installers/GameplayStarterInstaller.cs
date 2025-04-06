using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayStarterInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IGameplayStarterView>().To<GameplayStarterView>()
            .FromComponentsInHierarchy().AsCached();
    }
}   

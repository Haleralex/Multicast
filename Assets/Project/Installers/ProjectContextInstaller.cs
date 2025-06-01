using System.Collections;
using System.Collections.Generic;
using Core;
using Progress;
using Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ProjectContextInstaller : MonoInstaller<ProjectContextInstaller>
{
    public override void InstallBindings()
    {
        Container.Bind<IProgressManager>().To<ProgressManager>().AsSingle();
        Container.Bind<ISceneLoader>().To<AsyncSceneLoader>().AsSingle();
        Container.Bind<SettingsModel>().AsSingle();
    }
}
using System.Collections;
using System.Collections.Generic;
using Core;
using Progress;
using Settings;
using UnityEngine;
using Zenject;

public class ProjectContextInstaller : MonoInstaller<ProjectContextInstaller>
{
[SerializeField] private AudioManager audioManagerPrefab;
    
    public override void InstallBindings()
    {
        Container.Bind<IProgressManager>().To<ProgressManager>().AsSingle();
        Container.Bind<ISceneLoader>().To<AsyncSceneLoader>().AsSingle();

        Container.Bind<SettingsModel>().AsSingle();

        Container.Bind<IAudioManager>().To<AudioManager>()
            .FromComponentInNewPrefab(audioManagerPrefab)
            .AsSingle()
            .NonLazy();
    }
}
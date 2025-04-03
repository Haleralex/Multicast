using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    private LevelPresenter levelPresenter;

    public override void InstallBindings()
    {
        Container.Bind<ProgressManager>().AsTransient();
        Container.Bind<UIClusterFactory>().FromComponentInHierarchy().AsSingle();

        Container.Bind<LevelModel>().AsSingle();
        Container.Bind<LevelView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<LevelPresenter>().AsSingle().NonLazy();
    }
}
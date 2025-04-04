using System.Collections.Generic;
using UISystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<PlayGameView>().FromComponentInHierarchy().AsSingle().NonLazy();
        Container.Bind<PlayGameModel>().AsSingle();
        Container.Bind<PlayGamePresenter>().AsSingle().NonLazy();
    }
}

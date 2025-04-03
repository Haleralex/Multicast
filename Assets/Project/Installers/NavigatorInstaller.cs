using UnityEngine;
using Zenject;
using UISystem;
using Navigation;

public class NavigatorInstaller : MonoInstaller
{
    [SerializeField] private StartupConfig startupConfig;
    public override void InstallBindings()
    {
        Container.Bind<NavigationHistory>().AsSingle();

        Container.Bind<NavigatorModel>().AsSingle();
        Container.Bind<INavigatorView>()
            .To<NavigatorView>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<NavigatorPresenter>().AsSingle().NonLazy();
    }

    public override void Start()
    {
        var navigatorPresenter = Container.Resolve<NavigatorPresenter>();

        navigatorPresenter.SwitchToScreen(startupConfig.StartPanel);
    }
}

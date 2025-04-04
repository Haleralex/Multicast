using UnityEngine;
using Zenject;
using UISystem;
using Navigation;
using System.Collections.Generic;
using System.Linq;

public class NavigatorInstaller : MonoInstaller
{
    [SerializeField] private StartupConfig startupConfig;

    public override void InstallBindings()
    {
        Container.Bind<UIPanel>().FromComponentsInHierarchy().AsCached();

        Container.BindFactory<List<UIPanel>, PlaceholderFactory<List<UIPanel>>>()
            .FromMethod(CreateUIPanelList);

        Container.Bind<IUIPanelSwitcher>()
            .To<UIPanelSwitcher>().AsSingle();

        Container.Bind<NavigationHistory>().AsSingle();
        Container.Bind<NavigatorModel>().AsSingle();
        Container.Bind<INavigatorView>()
            .To<NavigatorView>()
            .FromComponentInHierarchy()
            .AsSingle();
        Container.Bind<NavigatorPresenter>().AsSingle().NonLazy();
    }

    private List<UIPanel> CreateUIPanelList(DiContainer context)
    {
        return Container.ResolveAll<UIPanel>().ToList();
    }

    public override void Start()
    {
        var navigatorPresenter = Container.Resolve<NavigatorPresenter>();

        navigatorPresenter.SwitchToScreen(startupConfig.StartPanel);
    }
}
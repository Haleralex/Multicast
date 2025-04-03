using UISystem;
using Zenject;

public class GameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IUIPanelSwitcher>()
            .To<UIPanelSwitcher>().FromComponentInHierarchy().AsSingle().NonLazy();
    }
}

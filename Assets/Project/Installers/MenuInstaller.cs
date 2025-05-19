using Core;
using UnityEngine;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IServicesStarter>().To<ServicesStarter>().AsSingle().NonLazy();
    }
}   

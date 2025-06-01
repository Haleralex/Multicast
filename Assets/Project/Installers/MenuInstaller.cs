using System.Threading.Tasks;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class MenuInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IServicesStarter>().To<ServicesStarter>().AsSingle().NonLazy();
    }    
}
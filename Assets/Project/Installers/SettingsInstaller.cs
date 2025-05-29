using UnityEngine;
using Zenject;
namespace Settings
{
    public class SettingsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
#if UNITY_ANDROID 
        Container.Bind<IVibrator>().To<AndroidVibrator>().AsSingle().NonLazy();
#endif
#if !UNITY_ANDROID
            Container.Bind<IVibrator>().To<MokVibrator>().AsSingle().NonLazy();
#endif
            Container.Bind<IVolumeSetter>().To<VolumeSetter>().AsTransient();
            Container.Bind<VibrationSetter>().AsTransient();

            Container.Bind<SettingsView>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.BindInterfacesAndSelfTo<SettingsPresenter>().AsSingle().NonLazy();
        }
    }
}
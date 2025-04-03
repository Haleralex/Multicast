using UnityEngine;
using Zenject;
namespace Settings
{
    public class SettingsInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        Container.Bind<IVibrator>().To<AndroidVibrator>().AsSingle().NonLazy();
#endif
#if UNITY_EDITOR
            Container.Bind<IVibrator>().To<MokVibrator>().AsSingle().NonLazy();
#endif
            Container.Bind<VolumeSetter>().AsTransient();
            Container.Bind<VibrationSetter>().AsTransient();

            Container.Bind<SettingsModel>().AsSingle();
            Container.Bind<SettingsView>()
                .FromComponentInHierarchy()
                .AsSingle();
            Container.Bind<SettingsPresenter>().AsSingle().NonLazy();
        }
    }
}
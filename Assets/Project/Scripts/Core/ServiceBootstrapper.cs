using System;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class ServiceBootstrapper : IServiceBootstrapper, IInitializable
{
    private string audioManagerKey = "AudioManager";
    private string audioSettingsKey = "AudioSettingsConfig";
    private string AnimationsConfigKey = "AnimationsConfig";
    private readonly DiContainer _container;

    public ServiceBootstrapper()
    {
        _container = ProjectContext.Instance.Container;
    }

    public async UniTask InitializeServicesAsync()
    {
        try
        {
            var audioSettingsTask = Addressables.LoadAssetAsync<AudioSettingsConfig>(audioSettingsKey);
            var audioManagerTask = Addressables.LoadAssetAsync<GameObject>(audioManagerKey);
            var animationsConfigTask = Addressables.LoadAssetAsync<AnimationsConfig>(AnimationsConfigKey);

            await UniTask.WhenAll(
                audioSettingsTask.ToUniTask(),
                audioManagerTask.ToUniTask(),
                animationsConfigTask.ToUniTask()
            );

            var audioSettingsConfig = audioSettingsTask.Result;
            var audioManagerPrefab = audioManagerTask.Result;
            var animationsConfig = animationsConfigTask.Result;

            RegisterAudioSettings(audioSettingsConfig);
            RegisterAudioManager(audioManagerPrefab);
            RegisterAnimationsConfig(animationsConfig);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to initialize services: {ex.Message}");
            throw;
        }
    }

    private void RegisterAudioSettings(AudioSettingsConfig audioSettingsConfig)
    {
        _container.Bind<AudioSettingsConfig>()
            .FromInstance(audioSettingsConfig)
            .AsSingle()
            .NonLazy();
    }

    private void RegisterAudioManager(GameObject audioManagerPrefab)
    {
        var audioManagerInstance = _container.InstantiatePrefab(
            audioManagerPrefab,
            Vector3.zero,
            Quaternion.identity,
            null);
        var audioManager = audioManagerInstance.GetComponent<AudioManager>();

        _container.Bind<IAudioManager>()
            .To<AudioManager>()
            .FromInstance(audioManager)
            .AsSingle()
            .NonLazy();
    }

    private void RegisterAnimationsConfig(AnimationsConfig animationsConfig)
    {
        _container.Bind<AnimationsConfig>()
            .FromInstance(animationsConfig)
            .AsSingle()
            .NonLazy();
    }

    public async void Initialize()
    {
        await InitializeServicesAsync();
    }
}
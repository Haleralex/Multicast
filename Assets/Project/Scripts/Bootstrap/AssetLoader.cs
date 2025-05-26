using System;
using System.Linq;
using Core;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

public class AssetLoader : IAssetLoader
{

    [SerializeField] private string audioManagerKey = "AudioManager";
    [SerializeField] private string audioSettingsKey = "AudioSettingsConfig";
    [SerializeField] private string AnimationsConfigKey = "AnimationsConfig";
    public event Action LoadingStarted;
    public event Action LoadingCompleted;
    public event Action<float> ProgressChanged;

    private readonly string[] _assetCategories = { "Game_Level", "Scenes", "Prefabs", "Audio" };

    private DiContainer _diContainer => ProjectContext.Instance.Container;

    public async UniTask LoadAssets(int fakeLoadDelayMilliSeconds = 0)
    {
        try
        {
            LoadingStarted?.Invoke();
            Debug.Log("Loading progress: OnLoadingStarted");

            // Load all categories in parallel
            await UniTask.WhenAll(_assetCategories.Select(LoadCategory));

            var audioSettingsConfig = await CreateAudioSettingsConfig();
            ProgressChanged?.Invoke(0.75f);
            
            _diContainer.Bind<AudioSettingsConfig>()
                            .FromInstance(audioSettingsConfig)
                            .AsSingle()
                            .NonLazy();


            var audioManager = await CreateAudioManager();
            _diContainer.Bind<IAudioManager>().To<AudioManager>()
                            .FromInstance(audioManager)
                            .AsSingle()
                            .NonLazy();

            var animationSettingsConfig = await CreateAnimationsConfig();
            _diContainer.Bind<AnimationsConfig>()
                            .FromInstance(animationSettingsConfig)
                            .AsSingle()
                            .NonLazy();


            ProgressChanged?.Invoke(1);

            if (fakeLoadDelayMilliSeconds > 0)
                await UniTask.Delay(fakeLoadDelayMilliSeconds);

            LoadingCompleted?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    private async UniTask LoadCategory(string label)
    {
        try
        {
            var operation = Addressables.DownloadDependenciesAsync(label);
            
            var progressReporter = new Progress<float>(progress =>
            {
                ProgressChanged?.Invoke(progress);
            });
            await operation.ToUniTask(progress: progressReporter);
            
            if (operation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to download {label}: {operation.OperationException}");
            }
            
            Addressables.Release(operation);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error loading category {label}: {ex.Message}");
            throw;
        }
    }

    private async UniTask<AudioManager> CreateAudioManager()
    {
        var prefabTask = Addressables.LoadAssetAsync<GameObject>(audioManagerKey);
        await prefabTask.ToUniTask();

        if (prefabTask.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Failed to load AudioManager: {prefabTask.OperationException}");
            return null;
        }

        var instance = _diContainer.InstantiatePrefab(prefabTask.Result);
        var audioManager = instance.GetComponent<AudioManager>();

        return audioManager;
    }

    private async UniTask<AudioSettingsConfig> CreateAudioSettingsConfig()
    {
        var configTask = Addressables.LoadAssetAsync<AudioSettingsConfig>(audioSettingsKey);
        await configTask.ToUniTask();

        if (configTask.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Failed to load AudioSettingsConfig: {configTask.OperationException}");
            return null;
        }

        return configTask.Result;
    }
    
    private async UniTask<AnimationsConfig> CreateAnimationsConfig()
    {
        var configTask = Addressables.LoadAssetAsync<AnimationsConfig>(AnimationsConfigKey);
        await configTask.ToUniTask();

        if (configTask.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Failed to load AudioSettingsConfig: {configTask.OperationException}");
            return null;
        }

        return configTask.Result;
    }
}

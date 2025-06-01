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

    public event Action LoadingStarted;
    public event Action LoadingCompleted;
    public event Action<float> ProgressChanged;

    private readonly string[] _assetCategories = { "Game_Level",
        "Scenes", "Prefabs", "Audio" };



    public async UniTask LoadAssets(int fakeLoadDelayMilliSeconds = 0)
    {
        try
        {
            LoadingStarted?.Invoke();
            Debug.Log("Loading progress: OnLoadingStarted");
            var progressAggregator = new ProgressAggregator(progress => ProgressChanged?.Invoke(progress));
            foreach (var category in _assetCategories)
            {
                progressAggregator.RegisterTracker(category);
            }

            var tasks = _assetCategories.Select(category =>
                LoadCategory(category, progress => progressAggregator.UpdateProgress(category, progress))).ToArray();

            await UniTask.WhenAll(tasks);

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

    private async UniTask LoadCategory(string label, Action<float> onProgress)
    {
        try
        {
            var operation = Addressables.DownloadDependenciesAsync(label);

            var progressReporter = new Progress<float>(progress =>
            {
                onProgress?.Invoke(progress);
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
}

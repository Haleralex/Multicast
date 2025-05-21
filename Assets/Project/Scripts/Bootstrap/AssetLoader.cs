using System;
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



    public async UniTask LoadAssets(int fakeLoadDelayMilliSeconds = 0)
    {
        try
        {
            LoadingStarted?.Invoke();
            Debug.Log($"Loading progress: OnLoadingStarted");
            
            await LoadCategory("Game_Level");
            await LoadCategory("Scenes");
            await LoadCategory("Prefabs");

            ProgressChanged?.Invoke(1);

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
        var operation = Addressables.DownloadDependenciesAsync(label);
        
        while (!operation.IsDone)
        {
            ProgressChanged?.Invoke(operation.PercentComplete);
            Debug.Log($"Loading {label} progress: {operation.PercentComplete * 100}%");
            await UniTask.Yield();
        }
        
        if (operation.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Failed to download {label}: {operation.OperationException}");
        }
        
        Addressables.Release(operation);
    }
}

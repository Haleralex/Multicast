using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

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
            var gameLevelOperation = Addressables.DownloadDependenciesAsync("Game_Level");

            while (!gameLevelOperation.IsDone)
            {
                ProgressChanged?.Invoke(gameLevelOperation.PercentComplete);
                Debug.Log($"Loading progress: {gameLevelOperation.PercentComplete * 100}%");
                await UniTask.Yield();
            }

            if (gameLevelOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to download assets: {gameLevelOperation.OperationException}");
            }

            Addressables.Release(gameLevelOperation);
            Debug.Log($"Loading progress: OnLoadingCompleted");

            var scenesOperation = Addressables.DownloadDependenciesAsync("Scenes");

            while (!scenesOperation.IsDone)
            {
                ProgressChanged?.Invoke(scenesOperation.PercentComplete);
                Debug.Log($"Loading progress: {scenesOperation.PercentComplete * 100}%");
                await UniTask.Yield();
            }

            if (scenesOperation.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to download assets: {scenesOperation.OperationException}");
            }

            Addressables.Release(scenesOperation);

            ProgressChanged?.Invoke(1);

            await UniTask.Delay(fakeLoadDelayMilliSeconds); 

            LoadingCompleted?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
}

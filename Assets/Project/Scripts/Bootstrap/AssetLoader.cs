using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetLoader
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
            var e = Addressables.DownloadDependenciesAsync("Game_Level");

            while (!e.IsDone)
            {
                ProgressChanged?.Invoke(e.PercentComplete);
                Debug.Log($"Loading progress: {e.PercentComplete * 100}%");
                await UniTask.Yield();
            }

            if (e.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError($"Failed to download assets: {e.OperationException}");
            }

            Addressables.Release(e);
            Debug.Log($"Loading progress: OnLoadingCompleted");
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

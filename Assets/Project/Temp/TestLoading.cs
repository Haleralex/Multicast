using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class TestLoading : MonoBehaviour
{
    async void Start()
    {
        var e = Addressables.DownloadDependenciesAsync("Temp");

        while (!e.IsDone)
        {
            await Task.Yield();
        }

        if (e.Status == AsyncOperationStatus.Failed)
        {
            Debug.LogError($"Failed to download assets: {e.OperationException}");
        }

        Addressables.Release(e);

        var go = await Addressables.InstantiateAsync("Temp").Task;

    }
}

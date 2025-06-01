using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BootstrapInitializer : IInitializable
{
    [Inject] private readonly IAssetLoader assetLoader;
    [Inject] private readonly MenuLoader menuLoader;
    [Inject] private readonly IServiceBootstrapper serviceBootstrapper;

    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private async UniTaskVoid InitializeAsync()
    {
        try
        {
            await assetLoader.LoadAssets();
            await serviceBootstrapper.InitializeServicesAsync();
            menuLoader.LoadMenu();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Ошибка при инициализации: {ex.Message}");
        }
    }
}

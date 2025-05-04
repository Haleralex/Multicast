using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class BootstrapInitializer : IInitializable
{
    [Inject] private readonly IAssetLoader assetLoader;
    [Inject] private readonly MenuLoader menuLoader;

    public void Initialize()
    {
        InitializeAsync().Forget();
    }

    private async UniTaskVoid InitializeAsync()
    {
        try
        {
            await assetLoader.LoadAssets(300);
            menuLoader.LoadMenu();
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Ошибка при инициализации: {ex.Message}");
        }
    }
}

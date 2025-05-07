using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace Core
{
    /// <summary>
    /// This class is responsible for loading scenes asynchronously.
    /// </summary>
    /// <remarks>
    /// It implements the ISceneLoader interface to provide methods for loading the gameplay and menu scenes.
    /// </remarks>
    public class AsyncSceneLoader : ISceneLoader
    {
        private SceneInstance currentSceneInstance;

        private const string GAMEPLAY_SCENE = "GamePlay";
        private const string MENU_SCENE = "Menu";

        public async UniTask LoadGameplayScene()
        {
            // Сначала выгружаем текущую активную сцену
            Scene currentScene = SceneManager.GetActiveScene();


            await Resources.UnloadUnusedAssets();
            System.GC.Collect();

            // Теперь загружаем новую сцену
            var loadOperation = Addressables.LoadSceneAsync(GAMEPLAY_SCENE, LoadSceneMode.Additive);

            // Ждем завершения загрузки
            await loadOperation.Task;

            if (loadOperation.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Gameplay scene loaded successfully.");
            }
            else
            {
                Debug.LogError($"Failed to load gameplay scene: {loadOperation.OperationException}");
            }

            if (currentScene.isLoaded && currentScene.name != GAMEPLAY_SCENE)
            {
                try
                {
                    // Проверяем, не первая ли это сцена, загруженная через билд настройки
                    if (currentScene.buildIndex == 0)
                    {
                        // Для первой сцены используем SceneManager
                        await SceneManager.UnloadSceneAsync(currentScene);
                        Debug.Log($"First scene {currentScene.name} unloaded successfully.");
                    }
                    else if (currentSceneInstance.Scene.isLoaded)
                    {
                        // Для сцен, загруженных через Addressables
                        var unloadHandle = Addressables.UnloadSceneAsync(currentSceneInstance);
                        await unloadHandle.Task;
                        Debug.Log($"Scene {currentSceneInstance.Scene.name} unloaded successfully.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to unload scene: {e.Message}");
                }
            }
            currentSceneInstance = loadOperation.Result;
        }

        public async UniTask LoadMenuScene()
        {
            // Сначала выгружаем текущую активную сцену
            Scene currentScene = SceneManager.GetActiveScene();


            await Resources.UnloadUnusedAssets();
            System.GC.Collect();

            // Теперь загружаем новую сцену
            var loadOperation = Addressables.LoadSceneAsync(MENU_SCENE, LoadSceneMode.Additive);

            // Ждем завершения загрузки
            await loadOperation.Task;

            if (loadOperation.Status == AsyncOperationStatus.Succeeded)
            {
                Debug.Log("Gameplay scene loaded successfully.");
            }
            else
            {
                Debug.LogError($"Failed to load gameplay scene: {loadOperation.OperationException}");
            }

            if (currentScene.isLoaded && currentScene.name != MENU_SCENE)
            {
                try
                {
                    // Проверяем, не первая ли это сцена, загруженная через билд настройки
                    if (currentScene.buildIndex == 0)
                    {
                        // Для первой сцены используем SceneManager
                        await SceneManager.UnloadSceneAsync(currentScene);
                        Debug.Log($"First scene {currentScene.name} unloaded successfully.");
                    }
                    else if (currentSceneInstance.Scene.isLoaded)
                    {
                        // Для сцен, загруженных через Addressables
                        var unloadHandle = Addressables.UnloadSceneAsync(currentSceneInstance);
                        await unloadHandle.Task;
                        Debug.Log($"Scene {currentSceneInstance.Scene.name} unloaded successfully.");
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"Failed to unload scene: {e.Message}");
                }
            }
            currentSceneInstance = loadOperation.Result;
        }
    }
}
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
            Scene currentScene = SceneManager.GetActiveScene();


            await Resources.UnloadUnusedAssets();
            System.GC.Collect();

            var loadOperation = Addressables.LoadSceneAsync(GAMEPLAY_SCENE, LoadSceneMode.Additive);

            await loadOperation.ToUniTask();

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
                    if (currentScene.buildIndex == 0)
                    {
                        await SceneManager.UnloadSceneAsync(currentScene);
                        Debug.Log($"First scene {currentScene.name} unloaded successfully.");
                    }
                    else if (currentSceneInstance.Scene.isLoaded)
                    {
                        var unloadHandle = Addressables.UnloadSceneAsync(currentSceneInstance);
                        await unloadHandle.ToUniTask();
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
            Scene currentScene = SceneManager.GetActiveScene();

            await Resources.UnloadUnusedAssets();
            System.GC.Collect();

            var loadOperation = Addressables.LoadSceneAsync(MENU_SCENE, LoadSceneMode.Additive);

            await loadOperation.ToUniTask();

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
                    if (currentScene.buildIndex == 0)
                    {
                        await SceneManager.UnloadSceneAsync(currentScene);
                        Debug.Log($"First scene {currentScene.name} unloaded successfully.");
                    }
                    else if (currentSceneInstance.Scene.isLoaded)
                    {
                        var unloadHandle = Addressables.UnloadSceneAsync(currentSceneInstance);
                        await unloadHandle.ToUniTask();
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
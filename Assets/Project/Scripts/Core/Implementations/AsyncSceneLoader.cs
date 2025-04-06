using System.Collections;
using UnityEngine;
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
        private const string GAMEPLAY_SCENE = "GamePlay";
        private const string MENU_SCENE = "Menu";
        public void LoadGameplayScene()
        {
            SceneManager.LoadSceneAsync(GAMEPLAY_SCENE, LoadSceneMode.Single);
        }

        public void LoadMenuScene()
        {
            SceneManager.LoadSceneAsync(MENU_SCENE, LoadSceneMode.Single);
        }
    }
}
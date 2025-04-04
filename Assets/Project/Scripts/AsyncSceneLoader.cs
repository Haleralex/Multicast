using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


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
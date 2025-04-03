using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Если вы планируете использовать UI для эффектов перехода

public class AsyncSceneLoader
{
    private const string GAMEPLAY_SCENE = "GamePlay";
    private const string MENU_SCENE = "Menu";
    public static void LoadGameplayScene()
    {
        SceneManager.LoadSceneAsync(GAMEPLAY_SCENE, LoadSceneMode.Single);
    }

    public static void LoadMenuScene()
    {
        SceneManager.LoadSceneAsync(MENU_SCENE, LoadSceneMode.Single);
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    [SerializeField] private AssetLoader assetLoader;
    void OnEnable()
    {
        assetLoader.LoadingCompleted += OnLoadingCompleted;
    }
    void OnDisable()
    {
        assetLoader.LoadingCompleted -= OnLoadingCompleted;
    }

    private void OnLoadingCompleted()
    {
        AsyncSceneLoader.LoadMenuScene();
    }
}

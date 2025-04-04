using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class AssetLoadingDisplay : MonoBehaviour, IInitializable
{
    [Inject] private readonly AssetLoader assetLoader;
    
    [SerializeField] private Slider loadingBar;
    [SerializeField] private TMPro.TextMeshProUGUI nameOfActivity;

    private const string ASSET_DOWNLOADING = "ASSET DOWNLOADING";
    private const string MENU_LOADING = "MENU LOADING";

    void OnEnable()
    {
        assetLoader.LoadingCompleted += OnLoadingCompleted;
        assetLoader.LoadingStarted += OnLoadingStarted;
        assetLoader.ProgressChanged += OnProgressChanged;
    }
    void OnDisable()
    {
        assetLoader.LoadingCompleted -= OnLoadingCompleted;
        assetLoader.LoadingStarted -= OnLoadingStarted;
        assetLoader.ProgressChanged -= OnProgressChanged;
    }

    public void Initialize()
    {
        SetStartCondition();
    }

    private void SetStartCondition()
    {
        loadingBar.gameObject.SetActive(false);
        nameOfActivity.text = string.Empty;
    }

    private void OnProgressChanged(float obj)
    {
        loadingBar.value = obj;
    }

    private void OnLoadingStarted()
    {
        loadingBar.gameObject.SetActive(true);
        nameOfActivity.text = ASSET_DOWNLOADING;
    }

    private void OnLoadingCompleted()
    {
        loadingBar.gameObject.SetActive(false);
        nameOfActivity.text = MENU_LOADING;
    }
}

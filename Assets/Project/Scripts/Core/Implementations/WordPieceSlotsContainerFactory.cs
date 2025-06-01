using System;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Zenject;

namespace Core.Implementations
{
    public class WordPieceSlotsContainerFactory : IWordPieceSlotsContainerFactory
    {
        private readonly DiContainer container;
        private readonly string containerAssetKey = "WordPieceSlotsContainer"; // ключ для Addressables
        private GameObject containerPrefab;
        private bool isInitialized = false;
        private AsyncOperationHandle<GameObject> loadHandle;

        private readonly Transform _parentTransform;

        [Inject]
        public WordPieceSlotsContainerFactory(DiContainer container, [Inject(Id = "SlotsContainersParent")] Transform parent = null)
        {
            _parentTransform = parent;
            this.container = container;
        }

        public async UniTask Initialize()
        {
            if (isInitialized)
            {
                Debug.LogError("WordPieceSlotsContainerFactory is already initialized.");
                return;
            }

            loadHandle = Addressables.LoadAssetAsync<GameObject>(containerAssetKey);
            await loadHandle.ToUniTask();

            if (loadHandle.Status == AsyncOperationStatus.Succeeded)
            {
                containerPrefab = loadHandle.Result;
                isInitialized = true;
            }
            else
            {
                Debug.LogError($"Failed to load WordPieceSlotsContainer prefab with key: {containerAssetKey}");
            }
        }

        public WordPieceSlotsContainer Create()
        {
            if (containerPrefab == null)
            {
                Debug.LogError("WordPieceSlotsContainer prefab is not loaded!");
                return null;
            }

            var container = this.container.InstantiatePrefabForComponent<WordPieceSlotsContainer>(
                    containerPrefab,
                    _parentTransform);

            if (container.TryGetComponent<RectTransform>(out var rectTransform))
            {
                rectTransform.anchorMin = new Vector2(0, 0);
                rectTransform.anchorMax = new Vector2(1, 0);
                rectTransform.pivot = new Vector2(0.5f, 0.5f);
                rectTransform.sizeDelta = new Vector2(0, rectTransform.sizeDelta.y);
                rectTransform.anchoredPosition = Vector2.zero;
            }

            return container;
        }

        public void Dispose()
        {
            containerPrefab = null;

            if (loadHandle.IsValid())
            {
                Addressables.ReleaseInstance(loadHandle);
                if(loadHandle.IsValid())
                Addressables.Release(loadHandle);

                loadHandle = default;
            }

            isInitialized = false;

            Resources.UnloadUnusedAssets();
        }
    }
}

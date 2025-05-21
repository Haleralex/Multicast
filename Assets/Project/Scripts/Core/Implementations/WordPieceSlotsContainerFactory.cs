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
    public class WordPieceSlotsContainerFactory : IWordPieceSlotsContainerFactory, IInitializable, IDisposable
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
                return;

            loadHandle = Addressables.LoadAssetAsync<GameObject>(containerAssetKey);
            await loadHandle.Task;

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

        public async UniTask<WordPieceSlotsContainer> CreateAsync()
        {
            if (containerPrefab == null)
            {
                Debug.LogError("WordPieceSlotsContainer prefab is not loaded!");
                return null;
            }

            var container = this.container.InstantiatePrefabForComponent<WordPieceSlotsContainer>(
                    containerPrefab,
                    _parentTransform);

            // Принудительное обновление LayoutGroup после добавления

            if (_parentTransform != null && _parentTransform.TryGetComponent<LayoutGroup>(out var layoutGroup))
            {
                // Форсируем перестроение Layout
                LayoutRebuilder.ForceRebuildLayoutImmediate(_parentTransform as RectTransform);
            }

            // Убедитесь, что RectTransform контейнера настроен правильно
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

            // Освобождаем ресурс Addressables
            if (loadHandle.IsValid())
            {
                // Принудительное освобождение всех созданных экземпляров
                Addressables.ReleaseInstance(loadHandle);

                // Для гарантии - освобождаем хендл напрямую
                Addressables.Release(loadHandle);

                // Обнуляем хендл
                loadHandle = default;
            }

            isInitialized = false;

            // Запрос на выгрузку неиспользуемых ресурсов
            Resources.UnloadUnusedAssets();
        }

        async void IInitializable.Initialize()
        {
            await Initialize();
        }
    }
}

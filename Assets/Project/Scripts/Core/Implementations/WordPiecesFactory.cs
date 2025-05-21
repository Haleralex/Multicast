using System;
using System.Threading.Tasks;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
namespace Core.Implementations
{
    public class WordPiecesFactory : IWordPiecesFactory, IInitializable
    {
        private readonly DiContainer _container;
        private readonly string wordPieceAssetKey = "WordPiece"; // ключ для Addressables
        private GameObject wordPiecePrefab;
        private bool isInitialized = false;
        private AsyncOperationHandle<GameObject> loadHandle;

        private readonly Transform _parentTransform;

        [Inject]
        public WordPiecesFactory(DiContainer container, [Inject(Id = "WordPiecesContainerParent")] Transform parent = null)
        {
            _container = container;
            _parentTransform = parent;
        }

        public async UniTask Initialize()
        {
            if (isInitialized)
                return;

            try
            {
                loadHandle = Addressables.LoadAssetAsync<GameObject>(wordPieceAssetKey);
                await loadHandle.Task;

                if (loadHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    wordPiecePrefab = loadHandle.Result;
                    isInitialized = true;
                }
                else
                {
                    Debug.LogError($"Failed to load WordPiece prefab with key: {wordPieceAssetKey}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Exception during WordPiece loading: {e.Message}");
                // Можно добавить повторную попытку или уведомление системы
            }
        }

        public async UniTask<WordPiece> CreateAsync()
        {
            if (wordPiecePrefab == null)
            {
                Debug.LogError("WordPiece prefab is not loaded!");
                return null;
            }

            return _container.InstantiatePrefabForComponent<WordPiece>(wordPiecePrefab, _parentTransform);
        }


        // Важно освободить ресурсы Addressables при уничтожении фабрики
        public void Dispose()
        {
            Debug.Log("Disposing WordPiecesFactory");

            // Обнуляем ссылку
            wordPiecePrefab = null;

            // Освобождаем ресурс Addressables
            if (loadHandle.IsValid())
            {
                // Принудительное освобождение всех созданных экземпляров
                Addressables.ReleaseInstance(loadHandle);

                // Для гарантии - освобождаем хендл напрямую
                if(loadHandle.IsValid())
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
            /* await Initialize(); */
        }
    }
}
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
    public class WordPiecesFactory : IWordPiecesFactory
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
            {
                Debug.Log("WordPiecesFactory is already initialized.");
                return;
            }

            try
            {
                loadHandle = Addressables.LoadAssetAsync<GameObject>(wordPieceAssetKey);
                await loadHandle.ToUniTask();

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
            }
        }

        public WordPiece Create()
        {
            if (wordPiecePrefab == null)
            {
                Debug.LogError("WordPiece prefab is not loaded!");
                return null;
            }

            return _container
                .InstantiatePrefabForComponent<WordPiece>(wordPiecePrefab, _parentTransform);
        }


        public void Dispose()
        {
            wordPiecePrefab = null;

            if (loadHandle.IsValid())
            {
                Addressables.ReleaseInstance(loadHandle);
                if (loadHandle.IsValid())
                    Addressables.Release(loadHandle);

                loadHandle = default;
            }

            isInitialized = false;

            Resources.UnloadUnusedAssets();
        }
    }
}
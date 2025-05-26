using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.Implementations
{
    public class WordPieceSlotsContainerPool
    {
        private readonly IWordPieceSlotsContainerFactory containerFactory;
        private int initialPoolSize = 4;
        private bool isInitialized = false;

        private List<WordPieceSlotsContainer> pooledContainers = new();

        [Inject]
        public WordPieceSlotsContainerPool(IWordPieceSlotsContainerFactory factory)
        {
            this.containerFactory = factory;
        }

        public async UniTask Initialize()
        {
            if (isInitialized)
            {
                Debug.LogError("WordPieceSlotsContainerPool is already initialized.");
                return;
            }

            await containerFactory.Initialize();

            for (int i = 0; i < initialPoolSize; i++)
            {
                pooledContainers.Add(CreateNewContainerAsync());
            }

            isInitialized = true;
        }

        private WordPieceSlotsContainer CreateNewContainerAsync()
        {
            var container = containerFactory.Create();
            if (container != null)
            {
                container.gameObject.SetActive(false);
            }
            return container;
        }

        public WordPieceSlotsContainer GetContainerAsync()
        {
            foreach (var container in pooledContainers)
            {
                if (!container.gameObject.activeSelf)
                {
                    container.gameObject.SetActive(true);
                    return container;
                }
            }

            var newContainer = CreateNewContainerAsync();
            newContainer.gameObject.SetActive(true);
            return newContainer;
        }

        public void ReturnContainer(WordPieceSlotsContainer container)
        {
            if (container != null)
            {
                container.ResetState();
                container.gameObject.SetActive(false);
            }
        }

        public void ClearPool()
        {
            isInitialized = false;
        }
    }
}
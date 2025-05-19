using System.Collections.Generic;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Core.Implementations
{
    public class WordPieceSlotsContainerPool : IInitializable
    {
        private readonly IWordPieceSlotsContainerFactory containerFactory;
        private readonly IWordPieceAnimator animator;
        private int initialPoolSize = 4;
        private bool isInitialized = false;

        private List<WordPieceSlotsContainer> pooledContainers = new();

        [Inject]
        public WordPieceSlotsContainerPool(IWordPieceSlotsContainerFactory factory,
            IWordPieceAnimator animator)
        {
            this.containerFactory = factory;
            this.animator = animator;
        }

        public async UniTask Initialize()
        {
            if (isInitialized)
                return;

            await containerFactory.Initialize();

            for (int i = 0; i < initialPoolSize; i++)
            {
                await CreateNewContainerAsync();
            }

            isInitialized = true;
        }

        private async UniTask<WordPieceSlotsContainer> CreateNewContainerAsync()
        {
            var container = await containerFactory.CreateAsync();
            if (container != null)
            {
                container.gameObject.SetActive(false);
                pooledContainers.Add(container);
            }
            return container;
        }

        public async UniTask<WordPieceSlotsContainer> GetContainerAsync()
        {
            while (!isInitialized)
                await UniTask.Yield();

            foreach (var container in pooledContainers)
            {
                if (!container.gameObject.activeSelf)
                {
                    // Временно без анимации
                    container.gameObject.SetActive(true);
                    return container;
                }
            }

            var newContainer = await CreateNewContainerAsync();
            // Временно без анимации
            newContainer.gameObject.SetActive(true);
            return newContainer;
        }

        public void ReturnContainer(WordPieceSlotsContainer container)
        {
            if (container != null)
            {
                container.ResetState();
                // Временно без анимации
                container.gameObject.SetActive(false);
            }
        }

        async void IInitializable.Initialize()
        {
            await Initialize();
        }
    }
}
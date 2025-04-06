using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    [SerializeField] private WordPiece wordPiecePrefab;
    [SerializeField] private Transform wordPiecesParent;
    public override void InstallBindings()
    {
        Container.Bind<IValidator>().To<Validator>().AsSingle();
        Container.BindFactory<WordPiece, WordPiecesFactory>()
            .FromComponentInNewPrefab(wordPiecePrefab)
            .UnderTransform(wordPiecesParent);

        Container.Bind<WordPiecesPool>().AsSingle();

        Container.Bind<WordPieceSlotsContainer>().FromComponentsInHierarchy().AsCached();

        Container.BindFactory<List<WordPieceSlotsContainer>, PlaceholderFactory<List<WordPieceSlotsContainer>>>()
            .FromMethod(CreateUIWordList);

        Container.Bind<LevelModel>().AsSingle();
        Container.Bind<ILevelView>().To<LevelView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ILevelInitializer>().To<LevelInitializer>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LevelPresenter>().AsSingle().NonLazy();

    }

    private List<WordPieceSlotsContainer> CreateUIWordList(DiContainer context)
    {
        return Container.ResolveAll<WordPieceSlotsContainer>().ToList();
    }
}
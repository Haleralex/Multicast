using System.Collections.Generic;
using System.Linq;
using Core.Implementations;
using Core.Interfaces;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    [SerializeField] private Transform wordPiecesParent;
    [SerializeField] private Transform slotsContainersParent;


    public override void InstallBindings()
    {
        Container.Bind<Transform>()
            .WithId("WordPiecesContainer")
            .FromInstance(wordPiecesParent);

        Container.Bind<Transform>()
                .WithId("SlotsContainersParent")
                .FromInstance(slotsContainersParent);

        Container.Bind<IValidator>().To<Validator>().AsSingle();
        Container.BindInterfacesAndSelfTo<WordPiecesFactory>().AsSingle();
        Container.BindInterfacesAndSelfTo<WordPieceSlotsContainerFactory>().AsSingle();

        Container.BindInterfacesAndSelfTo<WordPieceSlotsContainerPool>().AsSingle();
        Container.BindInterfacesAndSelfTo<WordPiecesPool>().AsSingle();

        Container.Bind<LevelModel>().AsSingle();
        Container.Bind<ILevelView>().To<LevelView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ILevelInitializer>().To<LevelInitializer>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LevelPresenter>().AsSingle().NonLazy();
        Container.Bind<IWordPieceAnimator>().To<WordPieceDOTweenAnimator>().AsSingle();
    }

    private List<WordPieceSlotsContainer> CreateUIWordList(DiContainer context)
    {
        return Container.ResolveAll<WordPieceSlotsContainer>().ToList();
    }
}
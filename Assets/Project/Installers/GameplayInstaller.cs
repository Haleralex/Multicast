using Core.Implementations;
using Core.Interfaces;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    [SerializeField] private Transform wordPiecesParent;
    [SerializeField] private Transform slotsContainersParent;
    [SerializeField] private Transform draggingParent;
    [SerializeField] private Canvas gameFieldCanvas;

    public override void InstallBindings()
    {
        Container.Bind<Transform>()
            .WithId("WordPiecesContainerParent")
            .FromInstance(wordPiecesParent);

        Container.Bind<Transform>()
                .WithId("SlotsContainersParent")
                .FromInstance(slotsContainersParent);

        Container.Bind<Transform>()
                .WithId("DraggingParent")
                .FromInstance(draggingParent);

        Container.Bind<Canvas>()
                .WithId("GameFieldCanvas")
                .FromInstance(gameFieldCanvas);

        Container.Bind<IValidator>().To<Validator>().AsSingle();
        Container.BindInterfacesAndSelfTo<WordPiecesFactory>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WordPieceSlotsContainerFactory>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<WordPiecesPool>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WordPieceSlotsContainerPool>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<WordPiecesView>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<WordSlotsView>().AsSingle().NonLazy();

        Container.Bind<IWordPieceSlotsService>().To<WordPieceSlotsService>().AsSingle();
        Container.Bind<IWordPiecesService>().To<WordPiecesService>().AsSingle();
        Container.Bind<ILevelAnimationService>().To<LevelAnimationService>().AsSingle();
        Container.Bind<IProgressRestoreService>().To<ProgressRestoreService>().AsSingle();

        Container.Bind<ILevelDataLoader>().To<LevelDataLoader>().AsSingle();
        Container.Bind<LevelModel>().AsSingle();
        Container.Bind<ILevelView>().To<LevelView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ILevelInitializer>().To<LevelInitializer>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<LevelPresenter>().AsSingle().NonLazy();
        Container.Bind<IWordPieceAnimator>().To<WordPieceDOTweenAnimator>().AsSingle();
        Container.Bind<IWordPieceSlotAnimator>().To<WordPieceSlotDOTweenAnimator>().AsSingle();
    }

}
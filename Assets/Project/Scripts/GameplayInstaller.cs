using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller<GameplayInstaller>
{
    [SerializeField] private UIClusterElement clusterElementPrefab;
    [SerializeField] private Transform clustersParent;
    public override void InstallBindings()
    {
        Container.Bind<IClusterManipulator>().To<ClusterManipulator>()
            .FromComponentInHierarchy().AsSingle();
        Container.BindFactory<UIClusterElement, UIClusterFactory>()
            .FromComponentInNewPrefab(clusterElementPrefab)
            .UnderTransform(clustersParent);

        Container.Bind<UIWord>().FromComponentsInHierarchy().AsCached();

        Container.BindFactory<List<UIWord>, PlaceholderFactory<List<UIWord>>>()
            .FromMethod(CreateUIWordList);

        Container.Bind<LevelModel>().AsSingle();
        Container.Bind<ILevelView>().To<LevelView>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ILevelInitializer>().To<LevelInitializer>().AsSingle().NonLazy();
        
        Container.BindInterfacesAndSelfTo<LevelPresenter>().AsSingle().NonLazy();

    }

    private List<UIWord> CreateUIWordList(DiContainer context)
    {
        return Container.ResolveAll<UIWord>().ToList();
    }
}
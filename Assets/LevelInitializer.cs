using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class LevelInitializer : ILevelInitializer
{
    [Inject] private readonly List<UIWord> words;
    [Inject] private readonly IClusterManipulator clusterManipulator;
    [Inject] private readonly UIClusterFactory clusterFactory;
    public void InitializeLevel(
        LevelData levelData, GameProgress progress = null)
    {
        var wordIndex = 0;
        int clusterIndex = 0;
        var emptyClusters = new List<UIWordEmptyCluster>();
        var wordlusters = new List<UIClusterElement>();
        
        foreach (var cluster in levelData.Words)
        {
            words[wordIndex].Initialize(cluster.Value.ToArray());
            emptyClusters.AddRange(words[wordIndex].ActiveEmptyClusters.ToList());
            wordIndex++;
            foreach (var word in cluster.Value)
            {
                var clusterElement = clusterFactory.Create();
                clusterElement.Initialize(word, clusterIndex++);
                wordlusters.Add(clusterElement);
            }
        }
        clusterManipulator.Initialize(emptyClusters, wordlusters);

        if (progress != null && progress.ClusterMapping.Count > 0)
        {
            clusterManipulator.HandleProgress(progress.ClusterMapping);
        }
    }
}

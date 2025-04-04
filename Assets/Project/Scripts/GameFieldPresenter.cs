using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameFieldPresenter
{
    private readonly GameFieldView view;
    private readonly GameFieldModel model;

    private Dictionary<int, UIWordEmptyCluster> _wordSlots = new();
    private Dictionary<int, UIClusterElement> _clusters = new();

    [Inject]
    public GameFieldPresenter(
        GameFieldView gameField,
        GameFieldModel clusterManipulator)
    {
        view = gameField;
        model = clusterManipulator;
    }

    public void Initialize()
    {
        view.ClusterSelected += OnClusterSelected;
        view.ClusterReleased += OnClusterReleased;
        view.ClusterDoubleClicked += OnClusterDoubleClicked;

        model.MappingChanged += UpdateUIFromMapping;
    }

    public void Dispose()
    {
        view.ClusterSelected -= OnClusterSelected;
        view.ClusterReleased -= OnClusterReleased;
        view.ClusterDoubleClicked -= OnClusterDoubleClicked;

        model.MappingChanged -= UpdateUIFromMapping;
    }

    public void InitializeField(List<UIWordEmptyCluster> wordSlots, List<UIClusterElement> clusters)
    {
        _wordSlots.Clear();
        _clusters.Clear();

        foreach (var slot in wordSlots)
        {
            _wordSlots[slot.Index] = slot;
        }

        foreach (var cluster in clusters)
        {
            _clusters[cluster.Index] = cluster;
        }

        view.Initialize();
    }

    private void OnClusterSelected(UIClusterElement cluster)
    {
        // Логика выбора кластера
    }

    private void OnClusterReleased(UIClusterElement cluster)
    {
        var closestSlot = FindClosestEmptySlot(cluster);
        if (closestSlot != null)
        {
            model.MapClusterToWord(cluster.Index, closestSlot.Index);
        }
        else
        {
            model.RemoveClusterMapping(cluster.Index);
        }
    }

    private void OnClusterDoubleClicked(UIClusterElement cluster)
    {
        model.RemoveClusterMapping(cluster.Index);
    }

    private UIWordEmptyCluster FindClosestEmptySlot(UIClusterElement cluster)
    {
        
        return null; // Заглушка
    }

    private void UpdateUIFromMapping(Dictionary<int, int> mapping)
    {
        // Сначала сбрасываем все слоты
        foreach (var slot in _wordSlots.Values)
        {
            slot.SetOccupied(false);
        }

        // Затем обновляем UI на основе маппинга
        foreach (var pair in mapping)
        {
            int clusterId = pair.Key;
            int slotId = pair.Value;

            if (_clusters.TryGetValue(clusterId, out var cluster) &&
                _wordSlots.TryGetValue(slotId, out var slot))
            {
                cluster.rectTransform.position = slot.rectTransform.position;
                cluster.ChangeParent(slot.transform);
                slot.SetOccupied(true);
            }
        }
    }
}

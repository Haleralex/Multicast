using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ClusterManipulator : MonoBehaviour, IClusterManipulator
{
    public event Action<Dictionary<int, int>> ClusterMappingUpdated;
    private RectTransform clusterCanvasRectTransform;
    [SerializeField] private Canvas clusterCanvas;
    private Dictionary<int, UIWordEmptyCluster> emptyClusters = new();
    private Dictionary<int, UIClusterElement> clusterElements = new();
    private UIClusterElement selectedClusterElement;
    private Dictionary<int, int> currentMapIndexes = new();

    private void StartCondition()
    {
        foreach(var k in emptyClusters.Keys.ToList())
        {
            emptyClusters[k].Clear();
        }
        foreach(var k in clusterElements.Keys.ToList())
        {
            Destroy(clusterElements[k].gameObject);
        }

        emptyClusters = new();
        clusterElements = new();
        currentMapIndexes = new();
    }

    public void Initialize(List<UIWordEmptyCluster> emptyClusters,
        List<UIClusterElement> clusterElements)
    {
        StartCondition();
        foreach (var emptyCluster in emptyClusters)
        {
            this.emptyClusters[emptyCluster.Index] = emptyCluster;
        }
        foreach (var clusterElement in clusterElements)
        {
            SubscribeToClusterEvents(clusterElement);
            this.clusterElements[clusterElement.Index] = clusterElement;
        }
        clusterCanvasRectTransform
            = clusterCanvas.GetComponent<RectTransform>();
    }
    private void SubscribeToClusterEvents(UIClusterElement clusterElement)
    {
        clusterElement.ClusterSelected += OnClusterSelected;
        clusterElement.ClusterReleased += OnClusterReleased;
        clusterElement.ClusterDoubleClicked += OnClusterDoubleClicked;
    }

    private void UnsubscribeFromClusterEvents(UIClusterElement clusterElement)
    {
        clusterElement.ClusterSelected -= OnClusterSelected;
        clusterElement.ClusterReleased -= OnClusterReleased;
        clusterElement.ClusterDoubleClicked -= OnClusterDoubleClicked;
    }

    public void HandleProgress(Dictionary<int, int> currentMapIndexes)
    {
        this.currentMapIndexes = currentMapIndexes;
        foreach (var clusterMap in currentMapIndexes)
        {
            UIClusterElement clusterElement = clusterElements[clusterMap.Key];
            UIWordEmptyCluster emptyCluster = emptyClusters[clusterMap.Value];

            clusterElement.rectTransform.position
                = emptyCluster.rectTransform.position;
            clusterElement.ChangeParent(emptyCluster.transform);

            emptyCluster.SetOccupied(true);
        }
    }

    public void OnClusterSelected(UIClusterElement clusterElement)
    {
        if (currentMapIndexes.ContainsKey(clusterElement.Index))
        {
            if (emptyClusters.TryGetValue(currentMapIndexes[clusterElement.Index],
                out var emptyCluster))
            {
                emptyCluster.SetOccupied(true);
            }
        }

        selectedClusterElement = clusterElement;
    }
    private void OnClusterDoubleClicked(UIClusterElement clusterElement)
    {
        if (currentMapIndexes.ContainsKey(clusterElement.Index))
        {
            if (emptyClusters.TryGetValue(currentMapIndexes[clusterElement.Index],
                out var emptyCluster))
            {
                emptyCluster.SetOccupied(false);
            }
            currentMapIndexes.Remove(clusterElement.Index);
            ClusterMappingUpdated?.Invoke(currentMapIndexes);
        }
        clusterElement.Return();
    }
    private void OnClusterReleased(UIClusterElement clusterElement)
    {
        MapCluster(clusterElement);
        selectedClusterElement = null;
    }

    private void MapCluster(UIClusterElement clusterElement)
    {
        if (selectedClusterElement == null)
        {
            Debug.LogError("No cluster selected to map.");
            return;
        }
        var selectedClusterElementRectTransform
            = selectedClusterElement.rectTransform;
        var orderedClusters = emptyClusters.OrderBy(
            a => (selectedClusterElementRectTransform.position - a.Value.rectTransform.position).magnitude).ToList();

        if (orderedClusters.Where(a => (selectedClusterElementRectTransform.position - a.Value.rectTransform.position).magnitude < 10 && !a.Value.IsOccupied).Count() == 0)
        {
            OnClusterDoubleClicked(clusterElement);
            return;
        }
        var closestCluster = orderedClusters[0];

        currentMapIndexes[clusterElement.Index]
            = closestCluster.Key;
        clusterElement.rectTransform.position
            = closestCluster.Value.rectTransform.position;
        clusterElement.ChangeParent(closestCluster.Value.transform);

        closestCluster.Value.SetOccupied(true);

        ClusterMappingUpdated?.Invoke(currentMapIndexes);
    }

    void Update()
    {
        if (selectedClusterElement == null)
            return;

        RectTransform rectTransform = selectedClusterElement.rectTransform;

        if (clusterCanvas == null)
        {
            Debug.LogError("Canvas not found!");
            return;
        }

        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            clusterCanvasRectTransform,
            Input.mousePosition,
            clusterCanvas.worldCamera,
            out Vector3 worldPos);

        Vector2 position = worldPos;

        rectTransform.position = new Vector3(position.x, position.y, rectTransform.position.z);
    }

    public void ResetClusters()
    {
        foreach (var cluster in currentMapIndexes.ToList())
        {
            OnClusterDoubleClicked(clusterElements[cluster.Key]);
        }

        currentMapIndexes.Clear();
        ClusterMappingUpdated?.Invoke(currentMapIndexes);
    }

    public string GetFullString()
    {
        var str = "";
        foreach (var cluster in currentMapIndexes.OrderBy(a => a.Value))
        {
            var clusterElement = clusterElements[cluster.Key];
            var emptyCluster = emptyClusters[cluster.Value];

            var fullString = clusterElement.Cluster;
            str += fullString;
        }
        return str;
    }
}

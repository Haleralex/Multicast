using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldView
{
    public event Action<UIClusterElement> ClusterSelected;
    public event Action<UIClusterElement> ClusterReleased;
    public event Action<UIClusterElement> ClusterDoubleClicked;
    
    [SerializeField] private Canvas clusterCanvas;
    private RectTransform _canvasRectTransform;
    private UIClusterElement _selectedCluster;
    
    public void Initialize()
    {
        _canvasRectTransform = clusterCanvas.GetComponent<RectTransform>();
    }
    
    // Публичные методы для вызова из UIClusterElement
    public void OnClusterSelect(UIClusterElement cluster)
    {
        _selectedCluster = cluster;
        ClusterSelected?.Invoke(cluster);
    }
    
    public void OnClusterRelease(UIClusterElement cluster)
    {
        _selectedCluster = null;
        ClusterReleased?.Invoke(cluster);
    }
    
    public void OnClusterDoubleClick(UIClusterElement cluster)
    {
        ClusterDoubleClicked?.Invoke(cluster);
    }
    
    // Обработка перемещения мыши
    private void Update()
    {
        if (_selectedCluster == null) return;
        
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            _canvasRectTransform,
            Input.mousePosition,
            clusterCanvas.worldCamera,
            out Vector3 worldPos);
        
        _selectedCluster.rectTransform.position = 
            new Vector3(worldPos.x, worldPos.y, _selectedCluster.rectTransform.position.z);
    }
}

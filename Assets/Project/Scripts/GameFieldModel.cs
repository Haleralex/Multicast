using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldModel : MonoBehaviour
{
    public event Action<Dictionary<int, int>> MappingChanged;
    
    private Dictionary<int, int> clusterWordMapping = new();
    
    public void MapClusterToWord(int clusterId, int wordId)
    {
        clusterWordMapping[clusterId] = wordId;
        MappingChanged?.Invoke(clusterWordMapping);
    }
    
    public void RemoveClusterMapping(int clusterId)
    {
        if (clusterWordMapping.ContainsKey(clusterId))
        {
            clusterWordMapping.Remove(clusterId);
            MappingChanged?.Invoke(clusterWordMapping);
        }
    }
    
    public void ResetMappings()
    {
        clusterWordMapping.Clear();
        MappingChanged?.Invoke(clusterWordMapping);
    }
    
    public Dictionary<int, int> GetCurrentMapping()
    {
        return new Dictionary<int, int>(clusterWordMapping);
    }
    
    public void InitializeFromProgress(Dictionary<int, int> savedMapping)
    {
        clusterWordMapping = new Dictionary<int, int>(savedMapping);
        MappingChanged?.Invoke(clusterWordMapping);
    }
}

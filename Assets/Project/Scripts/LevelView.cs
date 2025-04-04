using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelView : MonoBehaviour, ILevelView
{
    public event Action<GameProgress> GameFieldChanged;
    public event Action NextLevelPressed;
    public event Action ValidateLevelPressed;

    [SerializeField] private Button validateButton;

    [Inject] private readonly IClusterManipulator clusterManipulator;


    void OnEnable()
    {
        clusterManipulator.ClusterMappingUpdated += OnClusterMappingUpdated;
        validateButton.onClick.AddListener(ValidateLevelButtonPressed);
    }

    void OnDisable()
    {
        clusterManipulator.ClusterMappingUpdated -= OnClusterMappingUpdated;
        validateButton.onClick.RemoveListener(ValidateLevelButtonPressed);
    }

    private void OnClusterMappingUpdated(Dictionary<int, int> dictionary)
    {
        var newProgress = new GameProgress()
        {
            ClusterMapping = dictionary
        };
        GameFieldChanged?.Invoke(newProgress);
    }


    public void ValidateLevelButtonPressed()
    {
        ValidateLevelPressed?.Invoke();
    }

    public void SetNextLevel()
    {
        NextLevelPressed?.Invoke();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LevelView : MonoBehaviour
{
    [SerializeField] private ClusterManipulator clusterManipulator;
    [SerializeField] private UIClusterFactory clusterFactory;
    [SerializeField] private List<UIWord> words;
    [SerializeField] private Button resetButton;
    [SerializeField] private Button validateButton;
    [SerializeField] private GameObject levelCompletePanel;
    [Inject] private readonly LevelPresenter levelPresenter;

    void Start()
    {
        levelPresenter.Initialize();
    }

    void OnEnable()
    {
        clusterManipulator.ClusterMappingUpdated += OnClusterMappingUpdated;
        resetButton.onClick.AddListener(ResetProgress);
        validateButton.onClick.AddListener(ValidateLevel);
    }

    void OnDisable()
    {
        clusterManipulator.ClusterMappingUpdated -= OnClusterMappingUpdated;
        resetButton.onClick.RemoveListener(ResetProgress);
        validateButton.onClick.RemoveListener(ValidateLevel);
    }

    private void OnClusterMappingUpdated(Dictionary<int, int> dictionary)
    {
        var newProgress = new GameProgress()
        {
            CurrentLevel = levelPresenter.GetCurrentLevel(),
            ClusterMapping = dictionary
        };
        levelPresenter.UpdateProgress(newProgress);
    }
    public void InitializeLevel(LevelData levelData)
    {
        levelCompletePanel.SetActive(false);
        var wordIndex = 0;
        var emptyClusters = new List<UIWordEmptyCluster>();
        var wordlusters = new List<UIClusterElement>();
        int clusterIndex = 0;
        foreach (var cluster in levelData.Words)
        {
            words[wordIndex].Initialize(cluster.Value.ToArray());
            emptyClusters.AddRange(words[wordIndex].ActiveEmptyClusters.ToList());
            wordIndex++;
            foreach (var word in cluster.Value)
            {
                var clusterElement = clusterFactory.CreateClusterElement();
                clusterElement.Initialize(word, clusterIndex++);
                wordlusters.Add(clusterElement);
            }
        }
        clusterManipulator.Initialize(emptyClusters, wordlusters);
    }

    public void InitializeProgress(GameProgress currentProgress)
    {
        clusterManipulator.HandleProgress(currentProgress.ClusterMapping);
    }

    public void ResetProgress()
    {
        AsyncSceneLoader.LoadMenuScene();
    }

    public void ValidateLevel()
    {
        var validator = new Validator();
        var currentProgress = clusterManipulator.GetFullString();
        var fullWordToCompleteLevel = levelPresenter.GetFullWord();
        var isValid = validator.IsLevelValid(currentProgress, fullWordToCompleteLevel);

        if (isValid)
        {
            levelCompletePanel.SetActive(true);
        }
        else
        {
            Debug.Log("Level not completed yet.");
        }
    }

    public void SetNextLevel()
    {
        levelPresenter.SetNextLevel();
    }
}

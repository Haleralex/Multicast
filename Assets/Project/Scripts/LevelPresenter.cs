using UnityEngine;

using System.Collections.Generic;
using Zenject;
using System;

public class LevelPresenter : IInitializable
{
    private readonly LevelModel levelModel;
    private readonly ILevelView levelView;

    private readonly ILevelInitializer levelInitializer;

    [Inject]
    public LevelPresenter(
        LevelModel levelModel,
        ILevelView levelView,
        ILevelInitializer levelInitializer
        )
    {
        this.levelModel = levelModel;
        this.levelView = levelView;
        this.levelInitializer = levelInitializer;
    }

    public async void Initialize()
    {
        var currentProgress = levelModel.GetCurrentProgress();
        var currentLevelData = await levelModel.GetLevelData(currentProgress.CurrentLevel);
        levelInitializer.InitializeLevel(currentLevelData, currentProgress);
    }

    public int GetCurrentLevel()
    {
        var currentProgress = levelModel.GetCurrentProgress();
        return currentProgress.CurrentLevel;
    }

    public void UpdateProgress(GameProgress newProgress)
    {
        levelModel.UpdateProgress(newProgress);
    }

    public async void SetNextLevel()
    {
        var currentProgress = levelModel.GetCurrentProgress();
        currentProgress.CurrentLevel++;
        currentProgress.ClusterMapping.Clear();
        levelModel.UpdateProgress(currentProgress);

        var currentLevelData = await levelModel.GetLevelData(currentProgress.CurrentLevel);
        levelInitializer.InitializeLevel(currentLevelData);
    }

    public string GetFullWord()
    {
        return levelModel.GetWordForCompleteLevel();
    }
}
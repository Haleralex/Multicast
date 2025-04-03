using UnityEngine;

using System.Collections.Generic;
using Zenject;
using System;

public class LevelPresenter
{
    private readonly LevelModel levelModel;
    private readonly LevelView levelView;

    [Inject]
    public LevelPresenter(LevelModel levelModel, LevelView levelView)
    {
        this.levelModel = levelModel;
        this.levelView = levelView;
    }

    public async void Initialize()
    {
        var currentProgress = levelModel.GetCurrentProgress();
        var currentLevelData = await levelModel.GetLevelData(currentProgress.CurrentLevel);
        levelView.InitializeLevel(currentLevelData);
        levelView.InitializeProgress(currentProgress);
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
        levelView.InitializeLevel(currentLevelData);
    }

    public string GetFullWord()
    {
        return levelModel.GetWordForCompleteLevel();
    }
}
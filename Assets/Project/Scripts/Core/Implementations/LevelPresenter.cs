using UnityEngine;

using System.Collections.Generic;
using Zenject;
using System;
using Core;

public class LevelPresenter : IInitializable, IDisposable
{
    private readonly LevelModel model;
    private readonly ILevelView view;
    private readonly ILevelInitializer levelInitializer;
    private readonly IValidator validator;

    private readonly ISceneLoader sceneLoader;
    private Dictionary<int, string> fragmentsCache = new();

    [Inject]
    public LevelPresenter(
        LevelModel model,
        ILevelView view,
        ILevelInitializer levelInitializer,
        IValidator validator,
        ISceneLoader sceneLoader
        )
    {
        this.model = model;
        this.view = view;
        this.levelInitializer = levelInitializer;
        this.sceneLoader = sceneLoader;
        this.validator = validator;
    }

    public async void Initialize()
    {
        view.WordPieceSelected += OnWordPieceSelected;
        view.WordPieceReleased += OnWordPieceReleased;
        view.WordPieceDoubleClicked += OnWordPieceDoubleClicked;
        view.ValidateLevelPressed += OnValidateLevelPressed;
        view.NextLevelPressed += SetNextLevel;
        view.GoToMenuPressed += OnGoToMenuPressed;
        model.WordPiecesMappingChanged += OnWordPieceMappingChanged;

        var currentProgress = model.GetCurrentProgress();
        var currentLevelData = await model.GetCurrentLevelData();
        levelInitializer.InitializeLevel(currentLevelData.CorrectSlotsMapping, currentLevelData.MissingSlotsMapping, currentProgress);
    }



    private async void SetNextLevel()
    {
        ResetAllWordPiecesToInitialPositions();

        model.FinishLevel();

        var currentLevelData = await model.GetCurrentLevelData();
        levelInitializer.InitializeLevel(currentLevelData.CorrectSlotsMapping, currentLevelData.MissingSlotsMapping);

        model.ResetWordPieceMappings();
    }

    private void OnWordPieceSelected(WordPiece wordPiece)
    {
        OnWordPieceMappingChanged(MappingUpdate.Remove(wordPiece.Index));
    }

    private void OnWordPieceReleased(WordPiece wordPiece)
    {
        var closestSlot = view.FindClosestEmptySlot(wordPiece);
        if (closestSlot != null)
        {
            model.MapWordPieceToSlot(wordPiece.Index, closestSlot.Index);
        }
        else
        {
            model.RemoveWordPieceMapping(wordPiece.Index);
            view.ReturnWordPieceToInitialPosition(wordPiece);
        }
    }

    private void OnWordPieceDoubleClicked(WordPiece wordPiece)
    {
        model.RemoveWordPieceMapping(wordPiece.Index);
        view.ReturnWordPieceToInitialPosition(wordPiece);
    }

    private void OnWordPieceMappingChanged(MappingUpdate update)
    {
        var currentProgress = model.GetCurrentProgress();

        currentProgress.WordPiecesMapping ??= new Dictionary<int, int>();

        switch (update.Type)
        {
            case MappingUpdate.UpdateType.Add:
                Debug.Log($"Mapping update: {update.Type} for WordPiece {update.WordPieceId} to Slot {update.SlotId}");
                currentProgress.WordPiecesMapping[update.WordPieceId.Value] = update.SlotId.Value;
                break;

            case MappingUpdate.UpdateType.Remove:
                if (model.GetCurrentProgress().WordPiecesMapping.ContainsKey(update.WordPieceId.Value))
                {
                    currentProgress.WordPiecesMapping.Remove(update.WordPieceId.Value);
                    model.RemoveWordPieceMapping(update.WordPieceId.Value);
                }
                break;

            case MappingUpdate.UpdateType.Clear:

                break;

            case MappingUpdate.UpdateType.InitializeAll:
                if (model.IsLevelFull())
                    OnValidateLevelPressed();
                break;
        }

        model.UpdateProgress(currentProgress);

        view.UpdateUIFromMappings(model.WordPiecesMappings);
        if (model.IsLevelFull())
            OnValidateLevelPressed();
    }

    private void ResetAllWordPiecesToInitialPositions()
    {
        model.ResetWordPieceMappings();

        view.ResetAllWordPiecesToInitialPositions();

        var currentProgress = model.GetCurrentProgress();
        if (currentProgress.WordPiecesMapping != null)
        {
            currentProgress.WordPiecesMapping.Clear();
            model.UpdateProgress(currentProgress);
        }
    }



    private void OnValidateLevelPressed()
    {
        fragmentsCache.Clear();
        var currentMappings = model.WordPiecesMappings;
        string expectedWord = model.GetWordForCompleteLevel();

        foreach (var wordPiece in view.GetAllWordPieces())
        {
            fragmentsCache[wordPiece.Index] = wordPiece.Fragment;
        }

        bool isValid = validator.IsLevelValid(expectedWord, fragmentsCache, currentMappings);

        if (isValid)
        {
            view.ShowLevelCompletedMessage(model.GuessedWords);
        }
        else
        {
            Debug.Log("Неверное слово! Попробуйте еще раз.");
        }
    }
    private void OnGoToMenuPressed()
    {
        sceneLoader.LoadMenuScene();
    }

    public void Dispose()
    {
        // Отписка от событий View
        view.WordPieceSelected -= OnWordPieceSelected;
        view.WordPieceReleased -= OnWordPieceReleased;
        view.WordPieceDoubleClicked -= OnWordPieceDoubleClicked;
        view.ValidateLevelPressed -= OnValidateLevelPressed;
        view.NextLevelPressed -= SetNextLevel;
        view.GoToMenuPressed -= OnGoToMenuPressed;
        model.WordPiecesMappingChanged -= OnWordPieceMappingChanged;
    }


}
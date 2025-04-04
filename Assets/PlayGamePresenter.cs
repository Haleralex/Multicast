using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayGamePresenter 
{
    private readonly ISceneLoader sceneLoader;
    
    private readonly PlayGameModel model;
    private readonly PlayGameView view;

    [Inject]
    public PlayGamePresenter(PlayGameView view, PlayGameModel model, ISceneLoader sceneLoader)
    {
        this.sceneLoader = sceneLoader;
        this.model = model;
        this.view = view;

        this.view.PlayButtonClicked += OnPlayButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        StartGame();
    }

    public void StartGame()
    {
        sceneLoader.LoadGameplayScene();
    }
}

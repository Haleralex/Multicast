using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameplayStarterView : MonoBehaviour, IGameplayStarterView
{
    [SerializeField] private Button playButton;

    [Inject] private ISceneLoader sceneLoader;

    void OnEnable()
    {
        playButton.onClick.AddListener(Play);
    }

    void OnDisable()
    {
        playButton.onClick.RemoveListener(Play);
    }

    public void Play()
    {
        sceneLoader.LoadGameplayScene();
    }
}

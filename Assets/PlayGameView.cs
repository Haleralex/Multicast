using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class PlayGameView : MonoBehaviour
{
    public event Action PlayButtonClicked;
    [SerializeField] private Button playButton;

    void OnEnable()
    {
        playButton.onClick.AddListener(OnPlayButtonClicked);
    }

    void OnDisable()
    {
        playButton.onClick.RemoveListener(OnPlayButtonClicked);
    }


    private void OnPlayButtonClicked()
    {
        PlayButtonClicked?.Invoke();
    }
}

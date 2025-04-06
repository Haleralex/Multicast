using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompleteMessageView : MonoBehaviour
{
    public event Action NextLevelPressed;
    public event Action GoToMenuPressed;

    [SerializeField] private List<TMPro.TextMeshProUGUI> guessedWordLabels;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button goToMenuButton;

    void OnEnable()
    {
        nextLevelButton.onClick.AddListener(OnNextLevelButtonPressed);
        goToMenuButton.onClick.AddListener(OnGoToMenuButtonPressed);
    }

    void OnDisable()
    {
        nextLevelButton.onClick.RemoveListener(OnNextLevelButtonPressed);
        goToMenuButton.onClick.RemoveListener(OnGoToMenuButtonPressed);
    }

    private void OnGoToMenuButtonPressed()
    {
        GoToMenuPressed?.Invoke();
        
    }

    private void OnNextLevelButtonPressed()
    {
        NextLevelPressed?.Invoke();
        messagePanel.SetActive(false);
    }

    public void ShowMessage(IReadOnlyList<string> words)
    {
        if (words == null || words.Count == 0)
        {
            Debug.LogError("No words provided to display in the message panel.");
            return;
        }

        messagePanel.SetActive(true);

        if (guessedWordLabels.Count != words.Count)
        {
            Debug.LogError("Number of guessed word labels does not match the number of words provided.");
            return;
        }

        for (int i = 0; i < words.Count; i++)
        {
            guessedWordLabels[i].text = words[i];
        }
    }
}

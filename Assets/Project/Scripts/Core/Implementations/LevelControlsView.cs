using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelControlsView : MonoBehaviour
{
    public event Action GoToMenuPressed;

    [SerializeField] private Button goToMenuButton;

    void OnEnable()
    {
        goToMenuButton.onClick.AddListener(OnGoToMenuButtonPressed);
    }

    void OnDisable()
    {
        goToMenuButton.onClick.RemoveListener(OnGoToMenuButtonPressed);
    }

    private void OnGoToMenuButtonPressed()
    {
        GoToMenuPressed?.Invoke();
    }
}
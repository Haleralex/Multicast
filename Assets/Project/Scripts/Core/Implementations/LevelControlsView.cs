using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelControlsView : MonoBehaviour
{
    public event Action ValidateLevelPressed;
    public event Action NextLevelPressed;

    [SerializeField] private Button validateButton;

    private void OnEnable()
    {
        validateButton.onClick.AddListener(OnValidateButtonPressed);
    }

    private void OnDisable()
    {
        validateButton.onClick.RemoveListener(OnValidateButtonPressed);
    }

    private void OnValidateButtonPressed()
    {
        ValidateLevelPressed?.Invoke();
    }

    public void SetValidateButtonInteractable(bool isInteractable)
    {
        validateButton.interactable = isInteractable;
    }
}
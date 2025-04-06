using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelControlsView : MonoBehaviour
{
    public event Action ValidateLevelPressed;
    public event Action NextLevelPressed;

    [SerializeField] private Button validateButton;
    /* [SerializeField] private Button nextLevelButton; */

    private void OnEnable()
    {
        validateButton.onClick.AddListener(OnValidateButtonPressed);
        /* nextLevelButton.onClick.AddListener(OnNextLevelButtonPressed); */
    }

    private void OnDisable()
    {
        validateButton.onClick.RemoveListener(OnValidateButtonPressed);
        /* nextLevelButton.onClick.RemoveListener(OnNextLevelButtonPressed); */
    }

    private void OnValidateButtonPressed()
    {
        ValidateLevelPressed?.Invoke();
    }

    /* private void OnNextLevelButtonPressed()
    {
        NextLevelPressed?.Invoke();
    } */
}
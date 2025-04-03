using System;
using System.Collections;
using UISystem;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Navigation
{
    public class NavigatorView : MonoBehaviour, INavigatorView
    {
        [SerializeField] private List<UIPanelSwitcherButton> buttons;
        [SerializeField] private List<UIPanelBackButton> backButtons;
        public event Action<UIPanelType> ScreenButtonClicked;
        public event Action BackButtonClicked;

        void OnEnable()
        {
            foreach (var button in buttons)
                button.ButtonClicked += OnScreenSwitcherClicked;
            foreach (var button in backButtons)
                button.ButtonClicked += OnBackButtonClicked;
        }
        void OnDisable()
        {
            foreach (var button in buttons)
                button.ButtonClicked -= OnScreenSwitcherClicked;
            foreach (var button in backButtons)
                button.ButtonClicked -= OnBackButtonClicked;
        }

        private void OnScreenSwitcherClicked(UIPanelType screenType)
        {
            ScreenButtonClicked?.Invoke(screenType);
        }
        private void OnBackButtonClicked()
        {
            BackButtonClicked?.Invoke();
        }
    }
}
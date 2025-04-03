using System;
using UnityEngine;
using UISystem;
using Zenject;
namespace Navigation
{
    public class NavigatorPresenter : IDisposable
    {
        private readonly INavigatorView view;
        private readonly NavigatorModel model;
        private readonly NavigationHistory navigationHistory;
        private readonly IUIPanelSwitcher uIPanelSwitcher;

        [Inject]
        public NavigatorPresenter(INavigatorView view, NavigatorModel model,
            IUIPanelSwitcher uIPanelSwitcher, NavigationHistory navigationHistory)
        {
            this.view = view;
            this.model = model;
            this.uIPanelSwitcher = uIPanelSwitcher;
            this.navigationHistory = navigationHistory;

            view.ScreenButtonClicked += HandleStartButtonClicked;
            view.BackButtonClicked += HandleBackButtonClicked;
        }

        public void Dispose()
        {
            view.ScreenButtonClicked -= HandleStartButtonClicked;
            view.BackButtonClicked -= HandleBackButtonClicked;
        }

        private void HandleStartButtonClicked(UIPanelType screenType)
        {
            SwitchToScreen(screenType);
        }

        public void SwitchToScreen(UIPanelType screenType)
        {
            var curSkin = model.CurrentScreen;
            navigationHistory.Push(curSkin);
            model.SetCurrentScreen(screenType);
            uIPanelSwitcher.Switch(screenType);
        }

        private void HandleBackButtonClicked()
        {
            if (!navigationHistory.CanGoBack)
                return;

            if (navigationHistory.TryPop(out var popPanelType))
            {
                model.SetCurrentScreen(popPanelType);
                uIPanelSwitcher.Switch(popPanelType);
            }
        }
    }
}
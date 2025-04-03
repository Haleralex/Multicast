using UnityEngine;
using UISystem;
namespace Navigation
{
    public class NavigatorModel
    {
        public UIPanelType CurrentScreen { get; private set; }

        public void SetCurrentScreen(UIPanelType screen)
        {
            CurrentScreen = screen;
        }
    }
}
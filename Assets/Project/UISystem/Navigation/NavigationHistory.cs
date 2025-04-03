using System.Collections.Generic;
using UISystem;
using UnityEngine;
namespace Navigation
{
    public class NavigationHistory
    {
        private readonly Stack<UIPanelType> stackScreens = new();

        public void Push(UIPanelType screenType)
        {
            stackScreens.Push(screenType);
        }

        public bool TryPop(out UIPanelType pop)
        {
            if (stackScreens.TryPop(out var triedPop))
            {
                pop = triedPop;
                return true;
            }
            pop = UIPanelType.NoType;
            return false;
        }

        public bool CanGoBack => stackScreens.Count > 1;
    }
}
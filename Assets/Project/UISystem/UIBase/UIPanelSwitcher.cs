using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

namespace UISystem
{
    public class UIPanelSwitcher : IUIPanelSwitcher
    {
        private readonly Dictionary<UIPanelType, UIPanel> uIPanelsByType = new();

        [Inject]
        public UIPanelSwitcher(List<UIPanel> uIPanels)
        {
            foreach (var uiPanel in uIPanels)
            {
                uIPanelsByType[uiPanel.UIPanelType] = uiPanel;
            }
        }

        private void DisableAll()
        {
            foreach (var uiPanel in uIPanelsByType)
            {
                uiPanel.Value.Disable();
            }
        }

        public void Switch(UIPanelType type)
        {
            if (!uIPanelsByType.TryGetValue(type, out var panel))
                return;

            if (panel.IsEnabled)
                return;

            DisableAll();
            panel.Enable();
        }
    }
}
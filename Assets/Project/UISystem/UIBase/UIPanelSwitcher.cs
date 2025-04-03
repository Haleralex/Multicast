using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
namespace UISystem
{
    public class UIPanelSwitcher : MonoBehaviour, IUIPanelSwitcher
    {
        [SerializeField] private List<UIPanel> uIPanels = new();

        private Dictionary<UIPanelType, UIPanel> uIPanelsByType = new();

        void Awake()
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

        void IUIPanelSwitcher.Switch(UIPanelType type)
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
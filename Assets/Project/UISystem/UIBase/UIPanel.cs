using System;
using UnityEngine;

namespace UISystem
{
    public class UIPanel : MonoBehaviour
    {
        public event Action PanelEnabled;
        public event Action PanelDisabled;

        public UIPanelType UIPanelType;
        public bool IsEnabled { get; private set; } = false;
        public void Disable()
        {
            IsEnabled = false;
            gameObject.SetActive(false);

            PanelEnabled?.Invoke();
        }

        public void Enable()
        {
            IsEnabled = true;
            gameObject.SetActive(true);

            PanelDisabled?.Invoke();
        }
    }
}
using System;
using UnityEngine;
using UnityEngine.UI;

namespace UISystem
{
    public class UIPanelSwitcherButton : MonoBehaviour
    {
        public event Action<UIPanelType> ButtonClicked;
        [SerializeField] private Button button;
        public UIPanelType ScreenType;

        void OnEnable()
        {
            button.onClick.AddListener(OnButtonClick);
        }

        void OnDisable()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        void OnButtonClick()
        {
            button.OnSelect(null);
            ButtonClicked?.Invoke(ScreenType);
        }
    }
}
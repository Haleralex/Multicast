using System;
using UnityEngine;
using UnityEngine.UI;
namespace UISystem
{
    public class UIPanelBackButton : MonoBehaviour
    {
        public event Action ButtonClicked;
        [SerializeField] private Button button;

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
            ButtonClicked?.Invoke();
        }
    }
}
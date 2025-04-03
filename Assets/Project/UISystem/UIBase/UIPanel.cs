using UnityEngine;

namespace UISystem
{
    public class UIPanel : MonoBehaviour
    {
        public UIPanelType UIPanelType;
        public bool IsEnabled { get; private set; } = false;
        public void Disable()
        {
            gameObject.SetActive(false);
            IsEnabled = false;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
            IsEnabled = true;
        }
    }
}
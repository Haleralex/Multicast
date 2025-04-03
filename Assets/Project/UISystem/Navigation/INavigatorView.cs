using System;
using UISystem;
using UnityEngine;
namespace Navigation
{
    public interface INavigatorView
    {
        event Action<UIPanelType> ScreenButtonClicked;
        event Action BackButtonClicked;
    }
}
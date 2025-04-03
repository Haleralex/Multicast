using UISystem;
using UnityEngine;


[CreateAssetMenu(fileName = "StartupConfig", menuName = "Configs/StartupConfig")]
public class StartupConfig : ScriptableObject
{
    public UIPanelType StartPanel = UIPanelType.MainMenu;

}
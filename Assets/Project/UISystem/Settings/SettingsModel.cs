using UnityEngine;
namespace Settings
{
public class SettingsModel
{
    [Range(0, 100)]
    private float volumeLevel;
    private bool vibrationEnabled;

    public float VolumeLevel => volumeLevel;
    public bool VibrationEnabled => vibrationEnabled;

    public SettingsModel()
    {
        volumeLevel = PlayerPrefs.GetFloat("VolumeLevel", 1f);

        vibrationEnabled = PlayerPrefs.GetInt("VibrationEnabled", 1) == 1;
    }

    public void SetVolumeLevel(float value)
    {
        volumeLevel = value;
        PlayerPrefs.SetFloat("VolumeLevel", value);
    }

    public void SetVibrationEnabled(bool value)
    {
        vibrationEnabled = value;
        PlayerPrefs.SetInt("VibrationEnabled", value ? 1 : 0);
    }
}
}
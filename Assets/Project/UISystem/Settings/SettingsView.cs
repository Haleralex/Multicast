using System;
using UnityEngine;
using UnityEngine.UI;
namespace Settings
{
public class SettingsView : MonoBehaviour
{
    public event Action<float> VolumeSliderValueChanged;
    public event Action<bool> VibrationToggleValueChanged;

    [SerializeField] private Slider volumeSlider;
    [SerializeField] private Toggle vibrationToggle;

    void OnEnable()
    {
        volumeSlider.onValueChanged.AddListener(SetVolumeSliderValue);
        vibrationToggle.onValueChanged.AddListener(SetVibrationToggleValue);
    }

    void OnDisable()
    {
        volumeSlider.onValueChanged.RemoveListener(SetVolumeSliderValue);
        vibrationToggle.onValueChanged.RemoveListener(SetVibrationToggleValue);
    }

    private void SetVibrationToggleValue(bool value)
    {
        VibrationToggleValueChanged?.Invoke(value);
    }

    private void SetVolumeSliderValue(float value)
    {
        VolumeSliderValueChanged?.Invoke(value);
    }

    public void SetVolumeSliderValueWithoutNotification(float value)
    {
        volumeSlider.SetValueWithoutNotify(value);
    }

    public void SetVibrationToggleValueWithoutNotification(bool value)
    {
        vibrationToggle.SetIsOnWithoutNotify(value);
    }
}
}
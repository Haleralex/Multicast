using System;
using Core;
using UnityEngine;
using Zenject;
namespace Settings
{
    public class SettingsPresenter : IDisposable
    {
        private readonly SettingsView settingsView;
        private readonly SettingsModel settingsModel;
        private readonly IAudioManager volumeSetter;
        private readonly VibrationSetter vibrationSetter;

        [Inject]
        public SettingsPresenter(SettingsView settingsView, SettingsModel settingsModel,
         IAudioManager volumeSetter, VibrationSetter vibrationSetter)
        {
            this.settingsView = settingsView;
            this.settingsModel = settingsModel;
            this.volumeSetter = volumeSetter;
            this.vibrationSetter = vibrationSetter;

            settingsView.SetVolumeSliderValueWithoutNotification(settingsModel.VolumeLevel);
            settingsView.SetVibrationToggleValueWithoutNotification(settingsModel.VibrationEnabled);

            volumeSetter.SetVolumeValue(settingsModel.VolumeLevel);
            vibrationSetter.SetVibrationEnabled(settingsModel.VibrationEnabled);

            settingsView.VolumeSliderValueChanged += OnVolumeSliderValueChanged;
            settingsView.VibrationToggleValueChanged += OnVibrationToggleValueChanged;
        }

        private void OnVibrationToggleValueChanged(bool value)
        {
            settingsModel.SetVibrationEnabled(value);
            vibrationSetter.SetVibrationEnabled(settingsModel.VibrationEnabled);
        }

        private void OnVolumeSliderValueChanged(float value)
        {
            settingsModel.SetVolumeLevel(value);
            volumeSetter.SetVolumeValue(settingsModel.VolumeLevel);
        }

        public void Dispose()
        {
            settingsView.VolumeSliderValueChanged -= OnVolumeSliderValueChanged;
            settingsView.VibrationToggleValueChanged -= OnVibrationToggleValueChanged;
        }
    }
}

using UnityEngine;
using System;
using Zenject;
namespace Settings
{
public class VibrationSetter
{
    private readonly IVibrator vibrator;

    [Inject]
    public VibrationSetter(IVibrator vibrator)
    {
        this.vibrator = vibrator;
    }

    public void SetVibrationEnabled(bool value)
    {
        if (vibrator == null)
            return;
        vibrator.SetVibrationEnabled(value);
        vibrator.Vibrate(500);
    }
}

}

using UnityEngine;
namespace Settings
{
public class MokVibrator : IVibrator
{
    public void Cancel()
    {
        Debug.Log($"Vibrate cancelled");
    }

    public void SetVibrationEnabled(bool value)
    {
        Debug.Log($"Set Vibrator enabled = {value}");
    }

    public void Vibrate(long milliseconds = 500)
    {
        Debug.Log($"Vibrate {milliseconds} milliseconds");
    }
}
}
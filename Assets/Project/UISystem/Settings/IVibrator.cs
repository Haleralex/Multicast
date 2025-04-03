using UnityEngine;
namespace Settings
{
public interface IVibrator
{
    void Vibrate(long milliseconds = 500);
    void Cancel();

    void SetVibrationEnabled(bool value);
}
}
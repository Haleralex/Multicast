using System;
using UnityEngine;

namespace Settings
{
    public class AndroidVibrator : IVibrator
    {
        private bool enabled;
        private AndroidJavaObject vibrator;
        private AndroidJavaClass vibrationEffectClass;
        private int defaultAmplitude;

        public AndroidVibrator()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            InitializeVibrator();
#endif
        }

        private void InitializeVibrator()
        {
            try
            {
                using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                using (var activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                {
                    // Для Android 9+ (API 28+) используем vibrator_manager
                    if (GetAndroidSDKVersion() >= 28)
                    {
                        using (var vibratorManager = activity.Call<AndroidJavaObject>("getSystemService", "vibrator_manager"))
                        {
                            vibrator = vibratorManager?.Call<AndroidJavaObject>("getDefaultVibrator");
                        }
                    }
                    else
                    {
                        vibrator = activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
                    }

                    if (vibrator != null)
                    {
                        vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
                        defaultAmplitude = vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"Vibrator initialization error: {e.Message}");
            }
        }

        public void SetVibrationEnabled(bool value)
        {
            enabled = value;
            if (!enabled)
                Cancel();
        }

        public void Vibrate(long milliseconds = 500)
{
#if UNITY_ANDROID && !UNITY_EDITOR
    if (!enabled || vibrator == null) return;

    try
    {
        if (GetAndroidSDKVersion() >= 26) // Android 8.0+
        {
            using (var effect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                "createOneShot", 
                milliseconds, 
                defaultAmplitude))
            {
                vibrator.Call("vibrate", effect);
            }
        }
        else // Legacy vibration
        {
            vibrator.Call("vibrate", milliseconds);
        }
    }
    catch (Exception e)
    {
        Debug.LogError($"Vibration error: {e.Message}");
    }
#endif
}

        public void Cancel()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                vibrator?.Call("cancel");
            }
            catch (Exception e)
            {
                Debug.LogError($"Cancel vibration error: {e.Message}");
            }
#endif
        }

        private int GetAndroidSDKVersion()
        {
            using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
            {
                return version.GetStatic<int>("SDK_INT");
            }
        }
    }
}
using UnityEngine;

namespace Core
{
    public interface IAudioManager
    {
        void PlayBackgroundMusic();

        void PlayClip(AudioType audioType);

        void StopBackgroundMusic();

        void SetVolumeValue(float value);
    }
}

using UnityEngine;
using Zenject;
namespace Core
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private AudioSource mainAudioSource;
        [SerializeField] private AudioSource sideAudioSource;

        [Inject] private readonly AudioSettingsConfig _audioSettingsConfig;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (mainAudioSource != null)
                mainAudioSource.loop = true;
            if (sideAudioSource != null)
                sideAudioSource.loop = false;
        }

        public void PlayBackgroundMusic()
        {
            if (mainAudioSource == null)
                return;

            if(mainAudioSource.isPlaying)
                return;

            if (!_audioSettingsConfig.AudioClips.TryGetValue(AudioType.Menu, out var audioClip))
            {
                return;
            }

            mainAudioSource.PlayOneShot(audioClip);
        }

        public void StopBackgroundMusic()
        {
            if (mainAudioSource == null)
                return;

            mainAudioSource.Stop();
        }

        public void PlayClip(AudioType audioType)
        {
            if (sideAudioSource == null)
                return;

            if (!_audioSettingsConfig.AudioClips.TryGetValue(audioType, out var audioClip))
            {
                return;
            }

            sideAudioSource.PlayOneShot(audioClip);
        }

        public void SetVolumeValue(float value)
        {
            AudioListener.volume = value;
        }
    }

}
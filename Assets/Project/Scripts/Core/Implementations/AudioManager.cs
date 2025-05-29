using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
namespace Core
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        [SerializeField] private AudioSource mainAudioSource;
        [SerializeField] private AudioSource sideAudioSource;
        /* [Inject] private readonly IAudioLoader _audioLoader; */
        [Inject] private readonly AudioSettingsConfig _audioSettingsConfig;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            if (mainAudioSource != null)
                mainAudioSource.loop = true;
            if (sideAudioSource != null)
                sideAudioSource.loop = false;
        }

        public async void PlayBackgroundMusic()
        {
            if (mainAudioSource == null)
                return;

            if (mainAudioSource.isPlaying)
                return;

            if (!_audioSettingsConfig.AudioClips.TryGetValue(AudioType.Menu, out var audioClip))
            {
                return;
            }
            var testAudio = Addressables.LoadAssetAsync<AudioClip>(audioClip);
            await testAudio.ToUniTask();
            mainAudioSource.PlayOneShot(testAudio.Result);
        }

        public void StopBackgroundMusic()
        {
            if (mainAudioSource == null)
                return;

            mainAudioSource.Stop();
        }

        public async void PlayClip(AudioType audioType)
        {
            if (sideAudioSource == null)
                return;

            if (!_audioSettingsConfig.AudioClips.TryGetValue(audioType, out var audioClip))
            {
                return;
            }

            var testAudio = Addressables.LoadAssetAsync<AudioClip>(audioClip);
            await testAudio.ToUniTask();
            sideAudioSource.PlayOneShot(testAudio.Result);
        }

        public void SetVolumeValue(float value)
        {
            AudioListener.volume = value;
        }
    }

}
using UnityEngine;
namespace Core{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        private AudioSource audioSource;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            DontDestroyOnLoad(gameObject); // Сохраняем объект при смене сцен
        }

        public void PlayMenuMusic(AudioClip clip)
        {
            
        }

        public void StopMusic()
        {
            
        }
    }
}
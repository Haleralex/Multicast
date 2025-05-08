using UnityEngine;

namespace Core{


public interface IAudioManager
{
    public void PlayMenuMusic(AudioClip clip);

    public void StopMusic();
}
}
